using Networking.JsonObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Networking.Json
{
    public class JsonStartGame : JsonObject
    {
        public bool StartGame { get; set; }
        public JsonStartGame() : base()
        {
            MessageType = JsonMessageType.StartGame;
        }
    }
}
