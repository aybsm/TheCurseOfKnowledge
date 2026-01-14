using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TheCurseOfKnowledge.Core.Interfaces
{
    public interface IRabbitMQConsumer
    {
        string QueueName { get; }
        Task InitializeAsync(Dictionary<string, object> arguments, CancellationToken token);
        Task ListenAsync(Func<string, CancellationToken, Task> listenhandler, CancellationToken token);
        Task DestroyAsync(CancellationToken token);
    }
}
