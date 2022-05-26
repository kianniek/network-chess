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
            ChannelRequestDispatcher.Instance.SubscribeToMessageCallbackDispatcher(JsonMessageType.ChessMove, ForwardMessageClient);
        }

        private async void ForwardMessageClient(JsonObject jsonObject)
        {
            JsonChessMove json = jsonObject as JsonChessMove;
            Console.WriteLine(json.selectedCell.X + "," + json.selectedCell.Y + " : " + json.moveToCell.X + "," + json.moveToCell.Y + " \n" + json.currentPlayer);
            await ChannelRequestDispatcher.Instance.SendMessageExludeAsync(jsonObject, jsonObject.ChannelId);
        }
    }
}
