using Networking.Channels;
using Networking.Json;
using Networking.JsonObjects;
using System;

namespace Server
{
    internal class PlayerJoinedRequestHandler
    {
        int amountplayer;
        internal PlayerJoinedRequestHandler()
        {
            ChannelRequestDispatcher.Instance.SubscribeToMessageCallbackDispatcher(JsonMessageType.PlayerJoined, OnPlayerJoinedMessageReceived);
        }

        private async void OnPlayerJoinedMessageReceived(JsonObject jsonObject)
        {
            JsonRequestJoinGame jsonRequestJoinGame = jsonObject as JsonRequestJoinGame;
            if (amountplayer == 0)
            {
                await ChannelRequestDispatcher.Instance.SendMessageToAsync(new JsonAssignColor { ChessColor = ChessColor.White }, jsonRequestJoinGame.ChannelId);
            }
            else
            {
                await ChannelRequestDispatcher.Instance.SendMessageToAsync(new JsonAssignColor { ChessColor = ChessColor.Black }, jsonRequestJoinGame.ChannelId);
            }

            if (amountplayer == 1)
            {
                Console.WriteLine("GameStarted = true");
                await ChannelRequestDispatcher.Instance.SendMessageToAllAsync(new JsonStartGame { StartGame = true });
            }
            /**
             * Wanneer de Chess client wordt opgestart, wordt er direct een verzoek naar de server gestuurd om mee te mogen doen met het spel.
             * TODO: 
             * Stap 1: Stuur een berichtje terug naar de client waarin deze een kleur toegewezen krijgt.
             * Stap 2: Wanneer er twee spelers zijn verbonden, stuur een berichtje naar alle clients dat het spel gaat beginnen. Je kan spieken spieken bij de code van JsonAssigColor.
             */
            amountplayer++;
            Console.WriteLine("A new challenger approaches!");
        }
    }
}
