using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCurseOfKnowledge.Core.Extensions;
using TheCurseOfKnowledge.Core.Interfaces.Messaging;

namespace TheCurseOfKnowledge.Infrastructure.Messaging
{
    public class RabbitMQPublisher : IQueuePublisher
    {
        private readonly ObjectPool<IModel> __objectpool;

        public RabbitMQPublisher(ObjectPool<IModel> objectpolicy)
            => __objectpool = objectpolicy;
        public async Task PublishAsync<TModel>(TModel payload, string exchangetype, string routekey, Dictionary<string, object> arguments)
            where TModel : class
            => await PublishAsync(payload, routekey.Split('.').FirstOrDefault(), exchangetype, routekey, arguments);
        public async Task PublishAsync<TModel>(TModel payload, string exchangename, string exchangetype, string routekey, Dictionary<string, object> arguments)
            where TModel : class
            => await PublishAsync(JsonConvert.SerializeObject(payload), exchangename, exchangetype, routekey, arguments);
        public async Task PublishAsync(string payload, string exchangetype, string routekey, Dictionary<string, object> arguments)
            => await PublishAsync(payload, routekey.Split('.').FirstOrDefault(), exchangetype, routekey, arguments);
        public async Task PublishAsync(string payload, string exchangename, string exchangetype, string routekey, Dictionary<string, object> arguments)
        {
            Publish(payload, exchangename, exchangetype, routekey, arguments);
            await Task.Yield();
        }
        public void Publish<TModel>(TModel payload, string exchangetype, string routekey, Dictionary<string, object> arguments)
            where TModel : class
            => Publish(payload, routekey.Split('.').FirstOrDefault(), exchangetype, routekey, arguments);
        public void Publish<TModel>(TModel payload, string exchangename, string exchangetype, string routekey, Dictionary<string, object> arguments)
            where TModel : class
            => Publish(JsonConvert.SerializeObject(payload), exchangename, exchangetype, routekey, arguments);
        public void Publish(string payload, string exchangetype, string routekey, Dictionary<string, object> arguments)
            => Publish(payload, routekey.Split('.').FirstOrDefault(), exchangetype, routekey, arguments);
        public void Publish(string payload, string exchangename, string exchangetype, string routekey, Dictionary<string, object> arguments)
        {
            if (payload == null)
                return;
            var channel = __objectpool.Get();
            try
            {
                //channel.ExchangeDeclare(exchange: exchangename, type: exchangetype, durable: false, autoDelete: false, arguments: arguments);
                channel.BasicQos(prefetchSize: 0, prefetchCount: 15, global: false);
                var sendbytes = Encoding.UTF8.GetBytes(payload);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                channel.BasicPublish(exchange: exchangename, routingKey: routekey, basicProperties: properties, body: sendbytes);
            }
            catch (BrokerUnreachableException buex) { throw buex.GetDeepException(); }
            catch (RabbitMQClientException rex) { throw rex.GetDeepException(); }
            catch (Exception ex) { throw ex.GetDeepException(); }
            finally
            {
                __objectpool.Return(channel);
            }
        }
    }
}