using Networking.JsonObjects;
using System.Numerics;

namespace Networking.Json
{
    public class JsonChessMove : JsonObject
    {
        public ChessColor currentPlayer { get; set; }
        public Vector2 selectedCell { get; set; }
        public Vector2 moveToCell { get; set; }
        public JsonChessMove()
        {
            MessageType = JsonMessageType.ChessMove;
        }
    }
}
