using Networking.JsonObjects;
using Networking.Protocols;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Networking.Channels
{
    public class TcpChannel : IChannel
    {
        private readonly EndPoint remoteEndPoint;
        private readonly NetworkStream networkStream;
        private readonly TcpProtocol protocol;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private Action<JsonObject> onMessageReceived;
        private Action onConnectionClosed;
        
        public Guid ChannelId { get; private set; }
        public Action<JsonObject> OnMessageReceived { set => onMessageReceived = value; }
        public Action OnConnectionClosed { set => onConnectionClosed = value; }

        public ProtocolType ProtocolType => ProtocolType.Tcp;

        public EndPoint RemoteEndPoint => remoteEndPoint;

        public TcpChannel(Socket socket)
        {
            this.remoteEndPoint = socket.RemoteEndPoint;
            this.ChannelId = Guid.NewGuid();
            protocol = new TcpProtocol();
            networkStream = new NetworkStream(socket, true);
            _ = Task.Run(ReceiveLoop, cancellationTokenSource.Token);
        }

        private async Task ReceiveLoop()
        {
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    JsonObject message = await protocol.ReceiveMessageAsync(networkStream);
                    message.ChannelId = ChannelId;
                    onMessageReceived?.Invoke(message);
                }
                catch (IOException)
                {
                    Dispose();
                }
            }
        }

        public async Task SendAsync(JsonObject message)
        {
            message.ChannelId = ChannelId;
            await protocol.SendDataAsync(networkStream, message).ConfigureAwait(false);
        }

        ~TcpChannel() => Dispose();

        public void Dispose()
        {
            if (!cancellationTokenSource.Token.IsCancellationRequested)
            {                
                cancellationTokenSource.Cancel();
                networkStream?.Dispose();
                networkStream?.Close();
                onConnectionClosed?.Invoke();
            }
        }
    }    
}
