using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.GameObjects
{
    public class GameStateText : TextGameObject
    {
        public GameStateText(string assetname, int layer = 0, string id = "") : base(assetname, layer, id)
        {
            Color = Color.Black;
            text = "";
            Visible = false;
        }

        public void ShowCheckText(string text)
        {
            this.text = text;
            Position = new Vector2(GameEnvironment.Screen.X / 2 - Size.X, GameEnvironment.Screen.Y / 2 - Size.Y);
            Visible = true;
            Color = Color.Black;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Color newColor = Color;
            int alpha = Math.Max(newColor.A - 1, 0);
            newColor.A = (byte)alpha;
            Color = newColor;

            if (alpha <= 0)
            {
                visible = false;
            }
        }
    }
}
