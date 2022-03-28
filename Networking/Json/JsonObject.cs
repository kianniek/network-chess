using System;
using System.Net;

namespace Networking.JsonObjects
{
    public class JsonObject
    {
        public JsonMessageType MessageType { get; set; }
        public Guid ChannelId { get; set; }

        public EndPoint RemoteEndPoint { get; set; }

        public JsonObject()
        {
            
        }
    }
}
