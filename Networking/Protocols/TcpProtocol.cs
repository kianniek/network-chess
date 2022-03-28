using Networking.JsonObjects;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Networking.Protocols
{
    public class TcpProtocol
    {
        private InfrastructureDataSharingProtocol protocol;

        public TcpProtocol()
        {
            protocol = new InfrastructureDataSharingProtocol();
        }

        public async Task SendDataAsync(NetworkStream stream, JsonObject msg)
        {            
            byte[] output = protocol.PackageData(msg);
            await stream.WriteAsync(output, 0, output.Length);
        }

        public async Task<JsonObject> ReceiveMessageAsync(NetworkStream stream)
        {
            byte[] headerBytes = await ReadMessageAsync(stream, protocol.HEADER_SIZE).ConfigureAwait(false);
            (byte[] payloadSize, byte[] type) header = protocol.DecodeHeader(headerBytes);

            int payloadLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(header.payloadSize));
            byte[] type = header.type;

            if (payloadLength > 1)
            {
                byte[] payload = await ReadMessageAsync(stream, payloadLength).ConfigureAwait(false);
                JsonObject payloadObject = JsonObjectSerializer.Instance.DeserializeObject(payload, type);
                return payloadObject;
            }
            return null;
        }

        private async Task<byte[]> ReadMessageAsync(NetworkStream stream, int bytesToRead)
        {           
            byte[] buffer = new byte[bytesToRead];
            int bytesRead = 0;
            while (bytesRead < bytesToRead)
            {
                try
                {
                    int bytesReceived = await stream.ReadAsync(buffer, bytesRead, (bytesToRead - bytesRead)).ConfigureAwait(false);
                    if (bytesReceived == 0)
                    {
                        throw new Exception("Socket is closed.");
                    }
                    bytesRead += bytesReceived;
                }
                catch (SocketException exception)
                {
                    Debug.WriteLine("Something is going amiss...");
                }
            }
            return buffer;
        }
    }
}
