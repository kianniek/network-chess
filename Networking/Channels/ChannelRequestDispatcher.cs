using Networking.JsonObjects;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Networking.Channels
{
    public class ChannelRequestDispatcher
    {
        /**
         * Create a single static thread safe instance (AKA Singleton)
         * https://docs.microsoft.com/en-us/dotnet/api/system.lazy-1
         * https://refactoring.guru/design-patterns/singleton
         */
        private static readonly Lazy<ChannelRequestDispatcher> requestDispatcherInstance =
            new Lazy<ChannelRequestDispatcher>(() => new ChannelRequestDispatcher());

        /**
         * A thread safe key/value pair dictionary containing a list of listeners interested in incoming messages of specific types.
         * http://dotnetpattern.com/csharp-concurrentdictionary#:~:text=Final%20Words-,ConcurrentDictionary%20is%20thread%2Dsafe%20collection%20class%20to%20store%20key%2Fvalue,do%20CRUD%20operations%20on%20ConcurrentDictionary.
         */
        private ConcurrentDictionary<JsonMessageType, List<Action<JsonObject>>> messageCallbackDispatcher;

        /**
        * A thread safe key/value pair dictionary containing a channel identified by a unique id.        
        */
        private ConcurrentDictionary<Guid, IChannel> exchangeChannels;

        public static ChannelRequestDispatcher Instance => requestDispatcherInstance.Value;

        private ChannelRequestDispatcher()
        {
            exchangeChannels = new ConcurrentDictionary<Guid, IChannel>();
            messageCallbackDispatcher = new ConcurrentDictionary<JsonMessageType, List<Action<JsonObject>>>();
            InitializeCallbackDispatcher();
        }

        private void InitializeCallbackDispatcher()
        {            
            ForEachJsonMessageType(JsonMessageType.All, (JsonMessageType jsonMessageType) =>
                messageCallbackDispatcher.TryAdd(jsonMessageType, new List<Action<JsonObject>>()));
        }
        private void ForEachJsonMessageType(JsonMessageType subscriptionTypes, Action<JsonMessageType> callback)
        {
            IEnumerable<Enum> messageTypes = subscriptionTypes.GetSelectedFlags();

            foreach (JsonMessageType jsonMessageType in messageTypes)
            {
                callback(jsonMessageType);
            }
        }

        public void SubscribeToMessageCallbackDispatcher(JsonMessageType subscriptionTypes, Action<JsonObject> dispatcher) =>
            ForEachJsonMessageType(subscriptionTypes, (JsonMessageType jsonMessageType) =>
                messageCallbackDispatcher[jsonMessageType].Add(dispatcher));

        public void UnsbscribeFromMessageCallbackDispatcher(JsonMessageType subscriptionTypes, Action<JsonObject> dispatcher) =>
            ForEachJsonMessageType(subscriptionTypes, (JsonMessageType jsonMessageType) =>
                messageCallbackDispatcher[jsonMessageType].Remove(dispatcher));

        private void OnMessageReceived(JsonObject message)
        {
            List<Action<JsonObject>> callbacks;
            if (messageCallbackDispatcher.TryGetValue(message.MessageType, out callbacks))
            {
                callbacks.ForEach((Action<JsonObject> listener) =>
                {
                    listener(message);
                });
            }
        }

        public void AttachChannel(IChannel channel)
        {
            channel.OnMessageReceived = OnMessageReceived;
            exchangeChannels.TryAdd(channel.ChannelId, channel);

            channel.OnConnectionClosed = () =>
            {
                DetachChannel(channel);
            };
        }

        public void DetachChannel(IChannel channel)
        {
            Console.WriteLine($"{channel.ProtocolType} CONNECTION TERMINATED: {channel.ChannelId}");
            exchangeChannels.TryRemove(channel.ChannelId, out IChannel _);
            channel.Dispose();
        }

        public void DetachChannel(Guid networkId)
        {
            Console.WriteLine($"CONNECTION TERMINATED: {networkId}");
            if (exchangeChannels.TryRemove(networkId, out IChannel channel))
            {
                channel.Dispose();
            }            
        }

        /// <summary>
        /// Send a message to all attached channels.
        /// </summary>
        /// <param name="protocolType">the type of protocol, either tcp or udp.</param>
        /// <param name="message">the message to send. Must be of type JsonMessage or is inherited thereof.</param>
        /// <param name="SendToChannel">a function that returns true if the provided networkId should be notified, otherwise returns false.</param>
        private async Task SendMessageAsync(JsonObject message, Func<Guid, bool> SendToChannel)
        {
            foreach (KeyValuePair<Guid, IChannel> exchangeChannel in exchangeChannels)
            {
                IChannel channel = exchangeChannel.Value;
                if (SendToChannel(channel.ChannelId)/* && channel.RemoteEndPoint.ToString() != "0.0.0.0:0"*/)
                {
                    await channel.SendAsync(message);
                }
            }
        }

        public async Task SendMessageToAllAsync(JsonObject message)
        {
            await SendMessageAsync(message, (Guid networkId) =>
            {
                return true;
            });
        }

        public async Task SendMessageExludeAsync(JsonObject message, params Guid[] exclude)
        {
            await SendMessageAsync(message, (Guid networkId) =>
            {
                return !exclude.Contains<Guid>(networkId);
            });
        }

        public async Task SendMessageToAsync(JsonObject message, params Guid[] include)
        {
            await SendMessageAsync(message, (Guid networkId) =>
            {
                return include.Contains<Guid>(networkId);
            });
        }
    }
}
