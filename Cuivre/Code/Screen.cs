using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cuivre.Code
{
    public abstract class Screen
    {
        protected MouseState prevMouseState;
        protected Game1 gameInstance;

        public virtual void Init(Game1 game)
        {
            prevMouseState = Mouse.GetState();
            gameInstance = game;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
