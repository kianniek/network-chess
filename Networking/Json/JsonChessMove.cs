using Networking.JsonObjects;

namespace Networking.Json
{
    public class JsonChessMove : JsonObject
    {
        //public ChessColor ChessColor { get; set; }
        public JsonChessMove()
        {
            MessageType = JsonMessageType.ChessMove;
        }
    }
}
