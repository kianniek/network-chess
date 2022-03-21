using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameObjectList : GameObject
{
    protected List<GameObject> children;

    public GameObjectList(int layer = 0, string id = "") : base(layer, id)
    {
        children = new List<GameObject>();
    }

    public List<GameObject> Children
    {
        get { return children; }
    }

    public virtual void Add(GameObject obj)
    {
        obj.Parent = this;
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i].Layer > obj.Layer)
            {
                children.Insert(i, obj);
                return;
            }
        }
        children.Add(obj);
    }

    public void Remove(GameObject obj)
    {
        children.Remove(obj);
        obj.Parent = null;
    }

    public GameObject Find(string id)
    {
        foreach (GameObject obj in children)
        {
            if (obj.Id == id)
            {
                return obj;
            }
            if (obj is GameObjectList)
            {
                GameObjectList objList = obj as GameObjectList;
                GameObject subObj = objList.Find(id);
                if (subObj != null)
                {
                    return subObj;
                }
            }
        }
        return null;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        for (int i = children.Count - 1; i >= 0; i--)
        {
            children[i].HandleInput(inputHelper);
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        foreach (GameObject obj in children)
        {
            obj.Update(gameTime);
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (!visible)
        {
            return;
        }
        List<GameObject>.Enumerator e = children.GetEnumerator();
        while (e.MoveNext())
        {
            e.Current.Draw(gameTime, spriteBatch);
        }
    }

    public override void Reset()
    {
        base.Reset();
        foreach (GameObject obj in children)
        {
            obj.Reset();
        }
    }
}
