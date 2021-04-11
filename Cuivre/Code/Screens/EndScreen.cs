using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Cuivre.Code.Screens
{
    class EndScreen : Screen
    {
        private bool victory;
        private string highGauge;

        private bool hoverMenu = false;
        private bool hoverAgain = false;
        private Rectangle menuRect;
        private Rectangle againRect;

        private Dictionary<string, string> endTexts = new Dictionary<string, string>()
        {
            { "gameover", "Les Romains finissent par rejeter les valeurs et les rites apportes par Cybele. Des lois sont promulgues par les differents empereurs afin de reprimer le culte. Cybele finit par etre oubliee a Rome et n'est plus honoree." },
            { "Senateurs", "Le culte de Cybele devient un culte officiel de l'empire. Les empereurs l'incluent dans leurs projets de reforme religieuse et culturelle. Elle est utilisee dans des recits et discours officiels en tant que deesse eminemment romaine, et on finit meme par affirmer qu'elle etait romaine depuis le debut. On raconte qu'elle n'avait pas suivi Enee lorsqu'il avait fui la ville de Troie en ruine pour aller fonder Rome, mais qu'elle attendait le bon moment pour revenir dans sa ville de coeur. \n Cette fin s'est reellement produite au cours de l'Empire romain. Mais ce n'est qu'une partie de la realite. Pour en decouvrir plus..." },
            { "Philosophes", "Le culte de Cybele et les valeurs qu'elle transmet sont reutilises a foison dans la litterature philosophique. Les penseurs developpent meme de nouveaux aspects de Cybele qu'ils adaptent a leur philosophie. Lucrece la decrit comme celle qui contient en elle les elements fondamentaux du monde. Il l'utilise pour rendre plus ludiques certains concepts philosophiques. Cybele est enfin honoree comme la Terre Mere, celle qui est a l'origine des dieux et des hommes. \n Cette fin s'est reellement produite au cours de l'Empire romain. Mais ce n'est qu'une partie de la realite. Pour en decouvrir plus..." },
            { "Militaires", "Cybele devient un reel embleme de la Victoire. Elle est percue comme celle qui a aide Rome a gagner les guerres puniques contre Carthage. Lorsque son temple est construit, il se situe sur le Palatin, l'une des collines les plus importantes de Rome, juste a côte du sanctuaire de la Victoire. \n Cette fin s'est reellement produite au cours de l'Empire romain. Mais ce n'est qu'une partie de la realite. Pour en decouvrir plus..." },
            { "Amants", "Les serviteurs de Cybele perdent leur image de fanatiques a la sexualite devoyee, et sont de mieux en mieux consideres. L'arrivee de la deesse permet de s'interroger sur la conception de virilite romaine et elle influence les esprits, meme inconsciemment. Les Romains percoivent Cybele et ses serviteurs comme plus acceptables et moins choquants. \n Cette fin s'est reellement produite au cours de l'Empire romain. Mais ce n'est qu'une partie de la realite. Pour en decouvrir plus..." },
            { "Peuple", "Alors qu'auparavant certains rites en l'honneur de Cybele etaient interdits aux citoyens romains, ils peuvent desormais participer pleinement au culte. Cybele est consideree comme une deesse romaine a part entiere, et fait parfois l'objet de cultes personnels dans les foyers. \n Cette fin s'est reellement produite au cours de l'Empire romain. Mais ce n'est qu'une partie de la realite. Pour en decouvrir plus..." },
        };

        private string endText;

        public EndScreen(bool victory, string highGauge = "")
        {
            this.victory = victory;
            this.highGauge = highGauge;
        }
        public override void Init(Game1 game)
        {
            base.Init(game);

            MediaPlayer.Play(Game1.Musics["M_Final"]);
            MediaPlayer.IsRepeating = false;

            float ratio = Game1.Textures["bouton_jouer"].Height / (float)Game1.Textures["bouton_rejouer"].Width;
            int h = (int)(ratio * Game1.WIDTH / 4);
            againRect = new Rectangle(Game1.WIDTH / 2 - 8 * Game1.HEIGHT / 36, Game1.HEIGHT / 2 + GameScreen.leftOffset, Game1.WIDTH / 4, h);

            ratio = Game1.Textures["bouton_quitter"].Height / (float)Game1.Textures["bouton_menu"].Width;
            menuRect = new Rectangle(Game1.WIDTH / 2 - 7 * Game1.HEIGHT / 36, Game1.HEIGHT / 2 + h + GameScreen.leftOffset, Game1.WIDTH / 5, (int)(ratio * Game1.WIDTH / 5));

            if (!victory) endText = endTexts["gameover"];
            else endText = endTexts[highGauge];
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            string texture = "menu_";
            if (victory) texture += "victoire";
            else texture += "DEFAITE";
            spriteBatch.Draw(Game1.Textures[texture], new Rectangle(0, 0, Game1.WIDTH, Game1.HEIGHT), Color.White);

            Rectangle menu = new Rectangle(menuRect.Location, menuRect.Size);
            if (hoverMenu) menu = new Rectangle(menuRect.Location + new Point(0, -10), menuRect.Size + new Point(20, 20));
            spriteBatch.Draw(Game1.Textures["bouton_menu"], menu, Color.White);

            Rectangle again = new Rectangle(againRect.Location, againRect.Size);
            if (hoverAgain) again = new Rectangle(againRect.Location + new Point(0, -10), againRect.Size + new Point(20, 20));
            spriteBatch.Draw(Game1.Textures["bouton_rejouer"], again, Color.White);

            int y = Game1.HEIGHT / 4;
            foreach (string line in Utils.TextWrap.Wrap(endText, Game1.WIDTH / 2, Game1.font))
            {
                spriteBatch.DrawString(Game1.font, line, new Vector2(Game1.WIDTH / 4, y), Color.Black);
                y += (int)Game1.font.MeasureString("l").Y + 5;
            }
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            hoverAgain = false;
            hoverMenu = false;

            if (menuRect.Contains(mouseState.X, mouseState.Y))
            {
                hoverMenu = true;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    MenuScreen screen = new MenuScreen();
                    screen.Init(gameInstance);
                    gameInstance.ChangeScreen(screen);
                }
            }

            if (againRect.Contains(mouseState.X, mouseState.Y))
            {
                hoverAgain = true;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    GameScreen screen = new GameScreen();
                    screen.Init(gameInstance);
                    Gauges.gameEnd = false;
                    Miracle.ResetMiracleChance();
                    gameInstance.ChangeScreen(screen);
                }
            }
        }
    }
}
