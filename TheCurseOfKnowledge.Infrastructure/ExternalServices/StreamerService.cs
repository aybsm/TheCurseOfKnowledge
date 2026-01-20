using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheCurseOfKnowledge.Core.Extensions;
using TheCurseOfKnowledge.Core.Interfaces.ExternalServices;

namespace TheCurseOfKnowledge.Infrastructure.ExternalServices
{
    public class StreamerService : IStreamerService, IDisposable
    {
        readonly ClientWebSocket _ws;
        CancellationTokenSource _cts;
        public StreamerService(ClientWebSocket ws)
            => _ws = ws;
        public WebSocketState State
            => _ws.State;
        public async Task ConnectAsync(string url)
        {
            try
            {
                if (State == WebSocketState.Open || State == WebSocketState.Connecting)
                    await DisconnectAsync();
                _cts = new CancellationTokenSource();
                await _ws.ConnectAsync(new Uri(url), _cts.Token);
            }
            catch (Exception exc)
            { exc.GetDeepException(); }
        }
        public async Task ListenAsync<TModel>(Func<TModel, Task> listenhandler)
            => await ListenAsync(async (json) =>
            {
                try
                {
                    var model = JsonConvert.DeserializeObject<TModel>(json);
                    await listenhandler(model);
                }
                catch (Exception exc)
                { exc.GetDeepException(); }
            });
        public async Task ListenAsync(Func<string, Task> listenhandler)
        {
            byte[] buffer = ArrayPool<byte>.Shared.Rent(4096);
            try
            {
                while (!_cts.IsCancellationRequested)
                {
                    var result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), _cts.Token);
                    if (result.MessageType == WebSocketMessageType.Close)
                        break;
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    await listenhandler(message);
                }
            }
            catch (Exception exc)
            { exc.GetDeepException(); }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
                await DisconnectAsync();
            }
        }
        public async Task SendAsync(string message)
        {
            try
            {
                byte[] sendBytes = Encoding.UTF8.GetBytes(message);
                var sendBuffer = new ArraySegment<byte>(sendBytes);
                await _ws.SendAsync(sendBuffer, WebSocketMessageType.Text, true, _cts.Token);
            }
            catch (Exception exc)
            { exc.GetDeepException(); }
        }
        public async Task DisconnectAsync()
        {
            try
            {
                if (State == WebSocketState.Open)
                    await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "normal disconnect", _cts.Token);
                _cts?.Cancel();
            }
            catch (Exception exc)
            { exc.GetDeepException(); }
        }
        public void Dispose()
            => _cts?.Dispose();
    }
}
