using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TextGameObject : GameObject
{
    protected SpriteFont spriteFont;
    protected Color color;
    protected string text;

    public TextGameObject(string assetname, int layer = 0, string id = "")
        : base(layer, id)
    {
        spriteFont = GameEnvironment.AssetManager.Content.Load<SpriteFont>(assetname);
        color = Color.White;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (visible)
        {
            spriteBatch.DrawString(spriteFont, text, GlobalPosition, color, 0, new Vector2(0.5f, 0.5f), 2, SpriteEffects.None, 0);
        }
    }

    public Color Color
    {
        get { return color; }
        set { color = value; }
    }

    public string Text
    {
        get { return text; }
        set { text = value; }
    }

    public Vector2 Size
    {
        get
        { return spriteFont.MeasureString(text); }
    }
}