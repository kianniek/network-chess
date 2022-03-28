using System.Net;
using System.Net.Sockets;

namespace Server
{
    internal class FetchAddress
    {
        internal static IPAddress LocalAddress
        {
            get
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    return endPoint.Address;
                }
            }
        }
    }
}
