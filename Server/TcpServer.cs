using Networking.Channels;
using Server.RequestHandlers;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server
{
    public class TcpServer
    {       
        public TcpServer(IPEndPoint localEndPoint)
        {           
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(localEndPoint);
            socket.Listen(128);

            new PlayerJoinedRequestHandler();
            new ForwardMessageRequestHandler();

            Task.Run(() => ListenForIncomingTcpConnectionRequestsAsync(socket));
        }

        private async Task ListenForIncomingTcpConnectionRequestsAsync(Socket socket)
        {
            while (true)
            {
                Socket connectionSocket = await Task.Factory.FromAsync(
                new Func<AsyncCallback, object, IAsyncResult>(socket.BeginAccept),
                new Func<IAsyncResult, Socket>(socket.EndAccept),
                null).ConfigureAwait(false);

                TcpChannel channel = new TcpChannel(connectionSocket);
                ChannelRequestDispatcher.Instance.AttachChannel(channel);

                IPEndPoint remoteEndPoint = channel.RemoteEndPoint as IPEndPoint;

                Console.WriteLine($"TCP CONNECTION ESTABLISHED: {remoteEndPoint.Address}:{remoteEndPoint.Port}. CREATING CHANNEL {channel.ChannelId}.");
            }
        }
    }
}
