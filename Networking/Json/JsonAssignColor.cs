using Networking.JsonObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Networking.Json
{
    public class JsonAssignColor : JsonObject
    {
        public ChessColor ChessColor { get; set; }
        public JsonAssignColor() : base()
        {
            MessageType = JsonMessageType.AssignPlayerColor;
        }
    }
}
