﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
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
            })
        };

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            foreach (Button b in buttons)
            {
                b.Update(gameTime, prevMouseState, mouseState, this);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Button b in buttons)
            {
                b.Draw(gameTime, spriteBatch);
            }
        }
    }
}
