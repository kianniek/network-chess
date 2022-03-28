using Networking.JsonObjects;

namespace Networking.Json
{
    public class JsonRequestJoinGame : JsonObject
    {
        public ChessColor ChessColor { get; set; }
        public JsonRequestJoinGame() : base()
        {
            MessageType = JsonMessageType.PlayerJoined;
        }
    }
}
