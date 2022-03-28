using Client.GameStates;
using Client.Networking;
using Microsoft.Xna.Framework;
using Networking;
using System.Threading.Tasks;

namespace BaseProject
{
    public class Chess : GameEnvironment
    {      
        protected async override void LoadContent()
        {
            base.LoadContent();

            screen = new Point(850, 850);
            ApplyResolutionSettings();

            ChessBoard chessBoard = new ChessBoard();

            //TODO: use this.Content to load your game content here            
            GameStateManager.AddGameState("ChessBoard", chessBoard);
            GameStateManager.SwitchTo("ChessBoard");

            TcpConnector tcpConnector = new TcpConnector();
            //wait for the TCP connection to be established before initializing the requestListeners.
            await Task.Run(() => tcpConnector.MakeTcpConnectionAsync(FetchAddress.RemoteAddress));

            chessBoard.InitializeRequestHandlers();
        }
    }
}
