using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheCurseOfKnowledge.Core.Extensions;
using TheCurseOfKnowledge.Core.Interfaces.Messaging;
using TheCurseOfKnowledge.Infrastructure.Configurations;

namespace TheCurseOfKnowledge.Infrastructure.Messaging
{
    public class RabbitMQConsumer : IQueueConsumer, IDisposable
    {
        private readonly RabbitMQConfigModel __oOptions;
        private readonly ObjectPool<IModel> __oObjectpool;

        public RabbitMQConsumer(IOptions<RabbitMQConfigModel> optionsaccs, ObjectPool<IModel> objectpolicy)
        {
            __oOptions = optionsaccs.Value;
            __oObjectpool = objectpolicy;
        }
        public string QueueName
            => __oOptions.queue;
        public async Task InitializeAsync(Dictionary<string, object> arguments, CancellationToken token)
        {
            var __cChannel = __oObjectpool.Get();
            try
            {
                var __qQueues = (string.IsNullOrEmpty(value: __oOptions.exchangeroutingkey) ? __oOptions.queue : __oOptions.exchangeroutingkey).Split('.');
                if (__qQueues.Length != 3)
                    throw new ArgumentOutOfRangeException(paramName: "queuename", message: "queue name must be 3 section with dot (.) delimiter");
                __cChannel.ExchangeDeclare(exchange: __qQueues.First(), type: __oOptions.type, durable: false, autoDelete: false, arguments: arguments);
                __cChannel.QueueDeclare(queue: __oOptions.queue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: new Dictionary<string, object> { { "x-queue-type", "quorum" } });
                __cChannel.QueueBind(queue: __oOptions.queue, exchange: __qQueues.First(), routingKey: $"*.{__qQueues.Skip(1).First()}.{__qQueues.Last()}");
                __cChannel.BasicQos(prefetchSize: 0, prefetchCount: __oOptions.prefetchcount, global: false);

                await Task.CompletedTask;
            }
            catch (Exception exc) { throw exc.GetDeepException(); }
            finally
            {
                __oObjectpool.Return(__cChannel);
            }
        }
        public async Task ListenAsync(Func<string, CancellationToken, Task> listenhandler, CancellationToken token)
        {
            var __cChannel = __oObjectpool.Get();
            try
            {
                var __cConsumer = new EventingBasicConsumer(model: __cChannel);
                __cConsumer.Received += async (model, e) =>
                {
                    long deliveryCount = 0;
                    if (e.BasicProperties.Headers != null && e.BasicProperties.Headers.ContainsKey("x-delivery-count"))
                        deliveryCount = (long)e.BasicProperties.Headers["x-delivery-count"];

                    try
                    {
                        if (deliveryCount > 5)
                        {
                            __cChannel.BasicNack(e.DeliveryTag, false, requeue: false);
                            return;
                        }

                        var body = Encoding.UTF8.GetString(e.Body.ToArray());
                        await listenhandler(body, token);
                        __cChannel.BasicAck(e.DeliveryTag, false);
                    }
                    catch (Exception)
                    {
                        __cChannel.BasicNack(e.DeliveryTag, false, requeue: true);
                    }
                };
                __cChannel.BasicConsume(queue: __oOptions.queue, autoAck: false, consumer: __cConsumer);
                await Task.Delay(Timeout.Infinite, token);
            }
            catch (OperationCanceledException)
            {
                // Normal shutdown
            }
            catch (Exception exc)
            {
                throw exc.GetDeepException();
            }
            finally
            {
                __oObjectpool.Return(__cChannel);
            }
        }
        public async Task DestroyAsync(CancellationToken token)
        {
            Dispose();
            await Task.CompletedTask;
        }

        #region dispose
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }
        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~RabbitMQConsumer()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion dispose
    }
}
