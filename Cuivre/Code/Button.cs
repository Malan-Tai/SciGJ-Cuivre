using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cuivre.Code
{
    abstract class Button
    {
        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Click();
    }
}
