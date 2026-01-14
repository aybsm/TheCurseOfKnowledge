using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheCurseOfKnowledge.Core.Interfaces
{
    public interface IRabbitMQPublisher
    {
        Task PublishAsync<TModel>(TModel payload, string exchangename, string exchangetype, string routekey, Dictionary<string, object> arguments)
            where TModel : class;
        Task PublishAsync<TModel>(TModel payload, string exchangetype, string routekey, Dictionary<string, object> arguments)
            where TModel : class;
        Task PublishAsync(string payload, string exchangename, string exchangetype, string routekey, Dictionary<string, object> arguments);
        Task PublishAsync(string payload, string exchangetype, string routekey, Dictionary<string, object> arguments);
        void Publish<TModel>(TModel payload, string exchangename, string exchangetype, string routekey, Dictionary<string, object> arguments)
            where TModel : class;
        void Publish<TModel>(TModel payload, string exchangetype, string routekey, Dictionary<string, object> arguments)
            where TModel : class;
        void Publish(string payload, string exchangename, string exchangetype, string routekey, Dictionary<string, object> arguments);
        void Publish(string payload, string exchangetype, string routekey, Dictionary<string, object> arguments);
    }
}
