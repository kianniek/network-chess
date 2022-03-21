using Client.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BaseProject
{
    public class Chess : GameEnvironment
    {      
        protected override void LoadContent()
        {
            base.LoadContent();

            screen = new Point(850, 850);
            ApplyResolutionSettings();

            // TODO: use this.Content to load your game content here
            GameStateManager.AddGameState("ChessBoard", new ChessBoard());
            GameStateManager.SwitchTo("ChessBoard");
        }
    }
}
