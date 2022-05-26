using Client.GameStates;
using Networking.Channels;
using Networking.Json;
using Networking.JsonObjects;
using System.Diagnostics;
using System;

namespace Client.Networking
{
    internal class JoinGameRequestHandler
    {
        private IRequestListener requestListener;

        internal JoinGameRequestHandler(IRequestListener requestListener)
        {
            this.requestListener = requestListener;
            ChannelRequestDispatcher.Instance.SubscribeToMessageCallbackDispatcher(JsonMessageType.AssignPlayerColor, OnColorAssignedMessageReceived);

            DoRequestJoinGame();
        }

        private async void DoRequestJoinGame()
        { 
            await ChannelRequestDispatcher.Instance.SendMessageToAllAsync(new JsonRequestJoinGame());
        }

        private void OnColorAssignedMessageReceived(JsonObject jsonObject)
        {
            JsonAssignColor jsonAssignColor = jsonObject as JsonAssignColor;
            requestListener.ColorSelected(jsonAssignColor.ChessColor);
        }
    }
}
