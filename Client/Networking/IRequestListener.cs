using Networking.JsonObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.GameStates
{
    interface IRequestListener
    {
        void ColorSelected(ChessColor clientColor);
    }
}
