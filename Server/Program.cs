using System;
using System.Net;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint localEndPoint = new IPEndPoint(FetchAddress.LocalAddress, 4000);
            TcpServer tcpServer = new TcpServer(localEndPoint);
            Console.WriteLine($"TCP SERVER LISTENING ON: {localEndPoint.Address}:{localEndPoint.Port}");
            Console.ReadLine();
        }
    }
}
