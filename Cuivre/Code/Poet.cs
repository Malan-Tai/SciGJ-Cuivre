using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Cuivre.Code.Screens;

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

        private int x = Game1.WIDTH;
        private int speed = 1;
        private int minX = Game1.WIDTH - GameScreen.cardWidth - GameScreen.leftOffset;
        private int maxX = Game1.WIDTH;

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

        public void Init()
        {
            neutralTexture = Game1.Textures[TextureString];
            happyTexture = Game1.Textures[TextureString + "_happy"];
            unhappyTexture = Game1.Textures[TextureString + "_unappy"];
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
                //System.Diagnostics.Debug.WriteLine("happy " + TextureString);
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
            if (x < maxX)
            {
                float ratio = currentTexture.Height / (float)currentTexture.Width;
                int h = (int)(ratio * GameScreen.cardWidth);
                spriteBatch.Draw(currentTexture, new Rectangle(x, Game1.HEIGHT / 5, GameScreen.cardWidth, h), Color.White);
            }
            if (x <= minX)
            {
                DrawDialogue(spriteBatch);
            }
        }

        public void DrawDialogue(SpriteBatch spriteBatch)
        {
            Texture2D bubble = Game1.Textures["bulle_poete"];
            float ratio = bubble.Height / (float)bubble.Width;

            int textW = Game1.WIDTH - 2 * GameScreen.leftOffset - GameScreen.cardWidth;
            int textH = (int)(ratio * textW);
            int y = Game1.HEIGHT - textH - GameScreen.betweenOffset;

            spriteBatch.Draw(bubble, new Rectangle(GameScreen.leftOffset, y, textW, textH), Color.White);

            List<string> lines = Utils.TextWrap.Wrap(nextLine, textW - 2 * GameScreen.betweenOffset, Game1.font);

            y += textH / 5;
            foreach (string line in lines)
            {
                spriteBatch.DrawString(Game1.font, line, new Vector2(textW / 10 + GameScreen.leftOffset, y), Color.Black);
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
