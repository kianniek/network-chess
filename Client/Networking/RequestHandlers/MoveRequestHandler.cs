using Client.GameStates;
using Networking.Channels;
using Networking.Json;
using Networking.JsonObjects;
using System.Diagnostics;
using System;

namespace Client.Networking
{
    internal class MoveRequestHandler
    {
        private IRequestListener requestListener;

        internal MoveRequestHandler(IRequestListener requestListener)
        {
            this.requestListener = requestListener;
            ChannelRequestDispatcher.Instance.SubscribeToMessageCallbackDispatcher(JsonMessageType.ChessMove, OnChessMoveMessageReceived);
        }

        public async void DoChessMove(JsonObject jsonObject)
        {
            await ChannelRequestDispatcher.Instance.SendMessageToAllAsync(jsonObject);
        }

        private void OnChessMoveMessageReceived(JsonObject jsonObject)
        {
            JsonChessMove jsonChessMove = jsonObject as JsonChessMove;
            Console.WriteLine(jsonChessMove.selectedCell.X+","+ jsonChessMove.selectedCell.Y + " : " + jsonChessMove.moveToCell.X + "," + jsonChessMove.moveToCell.Y + " \n" + jsonChessMove.currentPlayer);
            requestListener.UpdateChessBord(jsonChessMove.selectedCell.X, jsonChessMove.selectedCell.Y, jsonChessMove.moveToCell.X, jsonChessMove.moveToCell.Y, jsonChessMove.currentPlayer);
        }
    }
}
