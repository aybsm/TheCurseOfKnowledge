using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using System;

namespace TheCurseOfKnowledge.Infrastructure.Messaging.Pooling
{
    public class RabbitMQChannelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly ObjectPool<IConnection> __connectionpool;

        public RabbitMQChannelPooledObjectPolicy(ObjectPool<IConnection> connectionPool)
            => __connectionpool = connectionPool;
        public IModel Create()
        {
            var connection = __connectionpool.Get();
            try
            {
                return connection.CreateModel();
            }
            finally
            {
                __connectionpool.Return(connection);
            }
        }
        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
                return true;
            obj?.Dispose();
            return false;
        }
    }
}
