using System.Numerics;
using Networking.JsonObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.GameStates
{
    interface IRequestListener
    {
        void ColorSelected(ChessColor clientColor);
        void UpdateChessBord(float XFrom, float YFrom, float XTo, float YTo, ChessColor currentPlayer);
        void StartGame(bool gameStarted);
    }
}
