using Client.GameStates;
using Networking.Channels;
using Networking.Json;
using Networking.JsonObjects;
using System.Diagnostics;
using System;

namespace Client.Networking
{
    internal class JoinGameRequestHandler
    {
        private IRequestListener requestListener;

        internal JoinGameRequestHandler(IRequestListener requestListener)
        {
            this.requestListener = requestListener;
            ChannelRequestDispatcher.Instance.SubscribeToMessageCallbackDispatcher(JsonMessageType.AssignPlayerColor, OnColorAssignedMessageReceived);

            DoRequestJoinGame();

            /**
             * TODO:
             * Stap 1: Maak een nieuwe JSON class aan en geef deze een handige naam. Dit is een berichtje dat de server stuurt naar de client  wanneer het spel begint.
             * Stap 2: Ga naar de class JsonObjectSerializer en voeg hier de code toe om een nieuw JSON object aan te maken. Je kan even spieken hoe dat gebeurd bij JsonAssignColor.
             * Tip: Nadat je het Json object hebt aangemaakt, zijn er nog 2 stappen die je moet doen om dit te laten werken.
             */
        }

        private async void DoRequestJoinGame()
        { 
            await ChannelRequestDispatcher.Instance.SendMessageToAllAsync(new JsonRequestJoinGame());
        }

        private void OnColorAssignedMessageReceived(JsonObject jsonObject)
        {
            JsonAssignColor jsonAssignColor = jsonObject as JsonAssignColor;
            requestListener.ColorSelected(jsonAssignColor.ChessColor);
            //Console.WriteLine(jsonAssignColor.ChessColor);
        }
    }
}
