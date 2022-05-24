using Networking.JsonObjects;
using System;
using System.Collections.Generic;
using System.Text;
using static Client.GameStates.ChessBoard;

namespace Client.GameStates
{
    interface IRequestListener
    {
        void ColorSelected(ChessColor clientColor);
        void UpdateChessBord(Cell from, Cell to);
    }
}
