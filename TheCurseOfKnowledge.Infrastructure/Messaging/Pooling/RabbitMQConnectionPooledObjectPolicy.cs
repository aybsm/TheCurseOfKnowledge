using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using TheCurseOfKnowledge.Infrastructure.Configurations;

namespace TheCurseOfKnowledge.Infrastructure.Messaging.Pooling
{
    public class RabbitMQConnectionPooledObjectPolicy : IPooledObjectPolicy<IConnection>
    {
        private readonly RabbitMQConfigModel __options;

        public RabbitMQConnectionPooledObjectPolicy(IOptions<RabbitMQConfigModel> optionsaccs)
            => __options = optionsaccs.Value;
        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                ClientProvidedName = __options.name ?? "TheCurseOfKnowledge",
                HostName = __options.hostname,
                UserName = __options.username,
                Password = __options.password,
                Port = __options.port,
                VirtualHost = __options.vhost,
                AutomaticRecoveryEnabled = __options.automaticrecoveryenabled,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(__options.networkrecoveryintervalinseconds),
            };
            return factory.CreateConnection();
        }
        public IConnection Create()
            => GetConnection();
        public bool Return(IConnection obj)
        {
            if (obj.IsOpen)
                return true;
            else
            {
                obj?.Dispose();
                return false;
            }
        }
    }
}
