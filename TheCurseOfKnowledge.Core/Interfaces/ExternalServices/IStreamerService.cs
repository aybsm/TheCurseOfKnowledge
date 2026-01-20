using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace TheCurseOfKnowledge.Core.Interfaces.ExternalServices
{
    public interface IStreamerService
    {
        WebSocketState State { get; }
        Task ConnectAsync(string url);
        Task ListenAsync<TModel>(Func<TModel, Task> listenhandler);
        Task ListenAsync(Func<string, Task> listenhandler);
        Task SendAsync(string message);
        Task DisconnectAsync();
        void Dispose();
    }
}
