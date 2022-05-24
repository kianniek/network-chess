using Client.GameStates;
using Networking.Channels;
using Networking.Json;
using Networking.JsonObjects;
using System.Diagnostics;
using System;

namespace Client.Networking
{
    internal class MoveRequestHandeler
    {
        private IRequestListener requestListener;

        internal MoveRequestHandeler(IRequestListener requestListener)
        {
            this.requestListener = requestListener;
            ChannelRequestDispatcher.Instance.SubscribeToMessageCallbackDispatcher(JsonMessageType.ChessMove, OnChessMoveMessageReceived);

            DoChessMove();
        }

        private async void DoChessMove()
        {
            await ChannelRequestDispatcher.Instance.SendMessageToAllAsync(new JsonChessMove());
        }

        private void OnChessMoveMessageReceived(JsonObject jsonObject)
        {
            JsonChessMove jsonChessMove = jsonObject as JsonChessMove;
            requestListener.UpdateChessBord(jsonChessMove);
            //Console.WriteLine(jsonChessMove);
        }
    }
}
