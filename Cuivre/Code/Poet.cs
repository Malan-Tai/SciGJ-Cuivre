using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

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
        private List<string> currentDialogues;
        private string nextLine;
        private List<string> soundEffectsList = new List<string>
        {
            "Poete1",
            "Poete2",
            "Poete3",
            //"Poete4"
        };

        public void Init(ContentManager content)
        {
            //neutralTexture = content.Load<Texture2D>(TextureString);
            //happyTexture = content.Load<Texture2D>(TextureString + "_happy");
            //unhappyTexture = content.Load<Texture2D>(TextureString + "_unhappy");
            neutralTexture = Game1.white;
            happyTexture = Game1.white;
            unhappyTexture = Game1.white;
            currentTexture = neutralTexture;
            currentDialogues = NeutralDialogues;

            //add another item for last said sentence in each dialogue category
            NeutralDialogues.Add("");
            UnhappyDialogues.Add("");
            HappyDialogues.Add("");
        }

        public void Call()
        {
            called = true;
            int gauge = Gauges.gaugesItems[GaugeName];

            Game1.Sounds[soundEffectsList[Utils.Dice.GetRandint(0, soundEffectsList.Count - 1)]].Play();

            if (gauge <= 25)
            {
                currentDialogues = UnhappyDialogues;
                currentTexture = unhappyTexture;
                //Game1.Sounds["Rite1"].Play();
            }
            else if (gauge > 75)
            {
                currentDialogues = HappyDialogues;
                currentTexture = happyTexture;
                //Game1.Sounds["Rite2"].Play();
            }
            else
            {
                currentDialogues = NeutralDialogues;
                currentTexture = neutralTexture;
                //Game1.Sounds["Rite3"].Play();
            }

            int n = currentDialogues.Count - 1; //number of real sentence, ignoring last said
            int i = n;
            while (currentDialogues[i].Equals(currentDialogues[n]))
            {
                i = Utils.Dice.GetRandint(0, n - 1);
            }
            nextLine = currentDialogues[i];
            currentDialogues[n] = nextLine;

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (x < maxX) spriteBatch.Draw(currentTexture, new Rectangle(x, 50, 100, 300), Color.White);
            if (x <= minX)
            {
                DrawDialogue(spriteBatch);
            }
        }

        public void DrawDialogue(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.white, new Rectangle(50, 300, 500, 50), Color.Wheat);

            int y = 310;
            foreach (string line in Utils.TextWrap.Wrap(nextLine, 480, Game1.font))
            {
                spriteBatch.DrawString(Game1.font, line, new Vector2(60, y), Color.Black);
                y += (int)Game1.font.MeasureString("l").Y + 5;
            }
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
