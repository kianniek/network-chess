using Networking.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Text;

namespace Networking.JsonObjects
{
    [Flags]
    public enum JsonMessageType
    {
        None                = 0b0000000000000000,

        PlayerJoined        = 0b0000000000000001,
        AssignPlayerColor   = 0b0000000000000010,        
        ChessMove           = 0b0000000000000100,
        StartGame           = 0b0000000000001000,

        All                 = 0b0000000000001111
    }

    public enum ChessColor
    {
        White,
        Black
    }

    public class JsonObjectSerializer
    {
        /**
         * Create a single static thread safe instance (AKA Singleton)
         * https://docs.microsoft.com/en-us/dotnet/api/system.lazy-1
         * https://refactoring.guru/design-patterns/singleton
         */
        private static readonly Lazy<JsonObjectSerializer> requestJsonSerializerInstance = 
            new Lazy<JsonObjectSerializer>(() => new JsonObjectSerializer());

        private readonly JsonSerializer jsonSerializer;
        private readonly JsonSerializerSettings jsonSettings;

        public static JsonObjectSerializer Instance => requestJsonSerializerInstance.Value;

        private JsonObjectSerializer()
        {
            jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy
                    {
                        ProcessDictionaryKeys = false
                    }
                }
            };
            jsonSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            jsonSerializer = JsonSerializer.Create(jsonSettings);
        }

        public JsonObject DeserializeObject(byte[] payload, byte[] type)
        {
            string message = Encoding.UTF8.GetString(payload);
            JObject jObject = JObject.Parse(message);

            JsonMessageType jsonMessageType = (JsonMessageType)BitConverter.ToInt32(type);
            switch (jsonMessageType)
            {
                case JsonMessageType.PlayerJoined:
                    return jObject.ToObject<JsonRequestJoinGame>();
                case JsonMessageType.AssignPlayerColor:
                    return jObject.ToObject<JsonAssignColor>();
                case JsonMessageType.StartGame:
                    return jObject.ToObject<JsonStartGame>();
                case JsonMessageType.ChessMove:
                    return jObject.ToObject<JsonChessMove>();
            }
            throw new Exception($"MessageType {jsonMessageType} not mapped.");
        }

        public void SerializeObject(StringWriter stringWriter, JsonObject jsonObject)
        {
            jsonSerializer.Serialize(stringWriter, jsonObject);
        }
    }
}
