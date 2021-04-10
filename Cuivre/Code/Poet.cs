using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cuivre.Code
{
    class Poet
    {
        public string Name { get; set; }

        public string GaugeName { get; set; }

        public string TextureString { get; set; }

        public List<string> HappyDialogues { get; set; }

        public List<string> NeutralDialogues { get; set; }

        public List<string> UnhappyDialogues { get; set; }

        private Texture2D neutralTexture;
        private Texture2D happyTexture;
        private Texture2D unhappyTexture;
        private Texture2D currentTexture;

        private int x = 800;
        private int speed = 1;
        private int minX = 600;
        private int maxX = 800;

        private bool called = false;

        public void Init(ContentManager content)
        {
            //neutralTexture = content.Load<Texture2D>(TextureString);
            //happyTexture = content.Load<Texture2D>(TextureString + "_happy");
            //unhappyTexture = content.Load<Texture2D>(TextureString + "_unhappy");
            neutralTexture = Game1.white;
            currentTexture = neutralTexture;
        }

        public void Call()
        {
            called = true;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (x < maxX) spriteBatch.Draw(currentTexture, new Rectangle(x, 50, 100, 300), Color.White);
        }

        public void Update(GameTime gameTime, MouseState mouseState)
        {
            if (called && x <= minX && mouseState.LeftButton == ButtonState.Pressed)
            {
                called = false;
            }

            if (called)
            {
                x = Math.Max(minX, x - (int)(speed * gameTime.ElapsedGameTime.TotalMilliseconds));
            }
            else if (x != maxX)
            {
                x = Math.Min(maxX, x + (int)(speed * gameTime.ElapsedGameTime.TotalMilliseconds));
            }
        }
    }
}
