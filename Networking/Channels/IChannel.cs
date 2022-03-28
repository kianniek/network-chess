using Networking.JsonObjects;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Networking.Channels
{
    public interface IChannel
    {
        /// <summary>
        /// A callback function that is called whenever a channel receives a new message.
        /// </summary>
        public Action<JsonObject> OnMessageReceived { set; }
        public Action OnConnectionClosed { set; }
        public ProtocolType ProtocolType { get; }
        public EndPoint RemoteEndPoint { get; }

        public void Dispose();

        /// <summary>
        /// the server determines the network id for each client.
        /// Each channel (TCP and UDP) gets its own unique identifier.
        /// </summary>
        public Guid ChannelId { get; }

        public Task SendAsync(JsonObject message);
    }
}
