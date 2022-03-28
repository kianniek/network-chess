using Networking.Channels;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Networking
{
    internal class TcpConnector
    {
        internal async Task MakeTcpConnectionAsync(EndPoint remoteEndPoint)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await socket.ConnectAsync(remoteEndPoint).ConfigureAwait(false);

            TcpChannel channel = new TcpChannel(socket);
            ChannelRequestDispatcher.Instance.AttachChannel(channel);
        }
    }
}