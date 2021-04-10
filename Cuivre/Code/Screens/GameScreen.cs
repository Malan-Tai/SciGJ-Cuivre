using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cuivre.Code.Screens
{
    class GameScreen : Screen
    {
        public Timeline Timeline { get; set; }

        public Dictionary<string, int> Gauges { get; set; }

        private List<Button> buttons = new List<Button>
        {
            new CollapseButton(5, 5, 70, 150, Game1.white, new List<Button>
            {
                new Button(10, 10, 50, 50, Game1.white, screen => { System.Diagnostics.Debug.WriteLine("pouet"); } ),
                new Button(10, 70, 50, 50, Game1.white, screen => { System.Diagnostics.Debug.WriteLine("pouet 2"); })
            }),
            new Button(70, 70, 50, 50, Game1.white, screen => { System.Diagnostics.Debug.WriteLine("pouet 3"); })
        };

        Poet poet = new Poet();

        public override void Init(ContentManager content)
        {
            base.Init(content);
            poet.Init(content);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.RightButton == ButtonState.Pressed) poet.Call();

            poet.Update(gameTime, mouseState);

            foreach (Button b in buttons)
            {
                b.Update(gameTime, prevMouseState, mouseState, this);
            }

            prevMouseState = mouseState;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            poet.Draw(gameTime, spriteBatch);

            foreach (Button b in buttons)
            {
                b.Draw(gameTime, spriteBatch);
            }
        }
    }
}
