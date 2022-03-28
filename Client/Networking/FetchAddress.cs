using System;
using System.IO;
using System.Net;

namespace Client.Networking
{
    internal class FetchAddress
    {
        internal static IPEndPoint RemoteAddress
        {
            get
            {
                string fileLocation = $"{Environment.CurrentDirectory}\\Content\\server_location.txt";
                string serverLocation = File.ReadAllText(fileLocation);

                Uri url = new Uri(serverLocation);
                IPAddress[] ipAddresses = Dns.GetHostAddresses($"{url.Host}");

                if (ipAddresses.Length > 0)
                    return new IPEndPoint(ipAddresses[0], url.Port);

                throw new Exception($"Host {url.AbsolutePath} not found.");
            }
        }
    }
}
