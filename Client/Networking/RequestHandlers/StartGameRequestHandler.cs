using Client.GameStates;
using Networking.Channels;
using Networking.Json;
using Networking.JsonObjects;
using System.Diagnostics;
using System;

namespace Client.Networking
{
    internal class StartGameRequestHandler
    {
        private IRequestListener requestListener;

        internal StartGameRequestHandler(IRequestListener requestListener)
        {
            this.requestListener = requestListener;
            ChannelRequestDispatcher.Instance.SubscribeToMessageCallbackDispatcher(JsonMessageType.StartGame, OnStartGameMessageReceived);
        }

        private void OnStartGameMessageReceived(JsonObject jsonObject)
        {
            JsonStartGame jsonStartGame = jsonObject as JsonStartGame;
            requestListener.StartGame(jsonStartGame.StartGame);
            Console.WriteLine(jsonStartGame.StartGame);
        }
    }
}
