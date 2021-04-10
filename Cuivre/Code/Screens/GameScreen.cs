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

        private List<Button> buttons = new List<Button>
        {
            new CollapseButton(5, 5, 70, 150, Game1.white, new List<Button>
            {
                new Button(10, 10, 50, 50, Game1.white, screen => { System.Diagnostics.Debug.WriteLine("pouet"); } ),
                new Button(10, 70, 50, 50, Game1.white, screen => { System.Diagnostics.Debug.WriteLine("pouet 2"); })
            }),
            new Button(70, 70, 50, 50, Game1.white, screen => { System.Diagnostics.Debug.WriteLine("pouet 3"); }),

            new Button(150, 100, 30, 30, Game1.white, screen => { Gauges.InitializeGauges(new List<string>(){"Philosophes","Millitaires", "Bidules"}); System.Diagnostics.Debug.WriteLine("Jauges initialisées"); }),
            
            //Pas très utile, mais je peux peut-être rajouter une sécurité si on ne trouve pas la clé dans le dico
            new Button(200, 100, 30, 30, Game1.white, screen => { Gauges.IncrementGaugeValue("Philosophes", 5);
                    System.Diagnostics.Debug.WriteLine("Les philosophes sont maintenant à :", Gauges.gaugesItems["Philosophes"]); }), //J'arrive pas à afficher la valeur en même temps , mais c'est pas grave
            new Button(250, 100, 30, 30, Game1.white, screen => { Gauges.IncrementGaugeValue("Millitaires", -10);
                    System.Diagnostics.Debug.WriteLine(Gauges.gaugesItems["Millitaires"]); }),
            new Button(300, 100, 30, 30, Game1.white, screen => { Gauges.IncrementGaugeValue("Bidules", 5);
                    System.Diagnostics.Debug.WriteLine(Gauges.gaugesItems["Bidules"]); })


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
