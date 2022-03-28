using Networking.JsonObjects;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Networking.Protocols
{
    public class InfrastructureDataSharingProtocol
    {
        private readonly int HEADER_SIZE_BYTES = 4;
        private readonly int HEADER_TYPE_BYTES = 4;
        public int HEADER_SIZE
        {
            get
            {
                return HEADER_TYPE_BYTES + HEADER_SIZE_BYTES;
            }
        }

        public InfrastructureDataSharingProtocol()
        {
            
        }

        public byte[] PackageData(JsonObject msg)
        {
            var (header, payload) = Encode(msg);

            byte[] output = new byte[header.Length + payload.Length];

            Array.Copy(header, 0, output, 0, header.Length);
            Array.Copy(payload, 0, output, header.Length, payload.Length);

            return output;
        }

        private byte[] GenerateHeader(byte[] payload, byte[] type)
        {
            //the header is always 5 bytes long. It contains the length of the payload byte array = 4 bytes (= 1 int) + type.
            byte[] header = new byte[HEADER_SIZE_BYTES + HEADER_TYPE_BYTES];

            //copy the size of the payload in bytes into the header.
            Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(payload.Length)), header, HEADER_SIZE_BYTES);
            Array.Copy(type, 0, header, HEADER_SIZE_BYTES, type.Length);
            return header;
        }

        private (byte[] header, byte[] payload) Encode(JsonObject message)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);

            JsonObjectSerializer.Instance.SerializeObject(sw, message);
           
            //convert the payload to a byte array.
            byte[] payload = Encoding.UTF8.GetBytes(sb.ToString());
            byte[] type = BitConverter.GetBytes((int)message.MessageType);
            byte[] header = GenerateHeader(payload, type);

            return (header, payload);
        }

        public (byte[] payloadSize, byte[] type) DecodeHeader(byte[] header)
        {
            byte[] payloadSize = new byte[HEADER_SIZE_BYTES];
            byte[] type = new byte[HEADER_TYPE_BYTES];
            Array.Copy(header, payloadSize, HEADER_SIZE_BYTES);
            Array.Copy(header, HEADER_SIZE_BYTES, type, 0, HEADER_TYPE_BYTES);
            return (payloadSize, type);
        }
    }
}