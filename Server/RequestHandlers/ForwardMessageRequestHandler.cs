using Networking.Channels;
using Networking.Json;
using Networking.JsonObjects;
using System;

namespace Server.RequestHandlers
{
    internal class ForwardMessageRequestHandler
    {
        internal ForwardMessageRequestHandler()
        {
            ChannelRequestDispatcher.Instance.SubscribeToMessageCallbackDispatcher(JsonMessageType.All, ForwardMessageClient);
        }

        private async void ForwardMessageClient(JsonObject jsonObject)
        {
            await ChannelRequestDispatcher.Instance.SendMessageExludeAsync(jsonObject, jsonObject.ChannelId);
        }
    }
}
