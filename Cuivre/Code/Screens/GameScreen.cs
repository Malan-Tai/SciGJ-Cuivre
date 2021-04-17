using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace Cuivre.Code.Screens
{
    class GameScreen : Screen
    {
        public Timeline Timeline { get; set; } = new Timeline();

        private Dictionary<string, Poet> poets;

        private bool newDay = false;
        private bool eventDay = false;

        public int currentEvent = 0;

        private bool readIntro;

        private int curLetter = 0;
        private int letterRate = 25;

        public const int leftOffset = 50;
        public const int cardNumbers = 5;
        public const int betweenOffset = 10;
        public static int cardWidth = (Game1.WIDTH - 2 * leftOffset - (cardNumbers - 1) * betweenOffset) / cardNumbers;

        private static float ratio = Game1.Textures["card_poetes"].Height / (float)Game1.Textures["card_poetes"].Width;

        private bool miracle = false;
        private bool miracleSuccess = false;

        private bool help = false;
        private Rectangle helpRectangle = new Rectangle(Game1.WIDTH - 3 * leftOffset / 2 - betweenOffset, Game1.HEIGHT / 12, 3 * leftOffset / 2, 3 * leftOffset / 2);

        public static int[] leftCoords = new int[] { 149 * Game1.HEIGHT / 1080, 363 * Game1.HEIGHT / 1080, 514 * Game1.HEIGHT / 1080, 668 * Game1.HEIGHT / 1080, 924 * Game1.HEIGHT / 1080, 1036 * Game1.HEIGHT / 1080 };
        public static int[] rightCoords = new int[] { 149 * Game1.HEIGHT / 1080, 274 * Game1.HEIGHT / 1080, 514 * Game1.HEIGHT / 1080, 757 * Game1.HEIGHT / 1080, 835 * Game1.HEIGHT / 1080, 1036 * Game1.HEIGHT / 1080 };

        private const int introWidthOffset = 400;
        private const int introHeightOffset = 200;
        private const int introCardWidth = 800;

        private int pulsingActions = 0;

        private List<Button> buttons = new List<Button>
        {

            //Bouton de l'oracle
            new Button(leftOffset + 3 * (cardWidth + betweenOffset), Game1.HEIGHT / 6, cardWidth, (int)(cardWidth * ratio), Game1.Textures["card_oracle"], screen => {
                if (((GameScreen)screen).SpendActionPoints(2, true))
                {
                    ((GameScreen)screen).Timeline.CallOracle();
                    //Game1.Sounds["Oracle"].Play();
                }}),

            //Methode de miracle appelée dans le SpendActionPoints pour tenir compte des PA
            new Button(leftOffset + 2 * (cardWidth + betweenOffset), Game1.HEIGHT / 6, cardWidth, (int)(cardWidth * ratio), Game1.Textures["card_miracle"], screen => {
                ((GameScreen)screen).SpendActionPoints(-1);
                ((GameScreen)screen).miracle = true;
                ((GameScreen)screen).curLetter = 0;
                if (Miracle.MiracleRoll())
                {
                    ((GameScreen)screen).miracleSuccess = true;
                    Game1.Sounds["Miracles"].Play();
                    List<string> keys = new List<string>(Gauges.gaugesItems.Keys);
                    foreach(string key in keys)
                    {
                        Gauges.IncrementGaugeValue(key, Miracle.gainedSatisfaction, screen);
                    }
                }
                else
                {
                    Game1.Sounds["MiracleRate"].Play();
                }}),

            //poetes
            new CollapseButton(leftOffset + cardWidth + betweenOffset, Game1.HEIGHT / 6, cardWidth, (int)(cardWidth * ratio), Game1.Textures["card_poetes"], Game1.Textures["card_poetes_verso"], true, new List<Button>
            {
                new Button(leftOffset + cardWidth, leftCoords[0], cardWidth, leftCoords[1] - leftCoords[0], Game1.Textures["juvenal_vignette"], screen => {
                ((GameScreen)screen).poets["Senateurs"].Call();}, true),

                new Button(leftOffset + cardWidth, rightCoords[1], cardWidth, leftCoords[2] - rightCoords[1], Game1.Textures["lucrece_vignette"], screen => {
                ((GameScreen)screen).poets["Philosophes"].Call();}, true),

                new Button(leftOffset + cardWidth, leftCoords[2], cardWidth, rightCoords[3] - rightCoords[2], Game1.Textures["virgile_vignette"], screen => {
                ((GameScreen)screen).poets["Militaires"].Call();}, true),

                new Button(leftOffset + cardWidth, leftCoords[3], cardWidth, leftCoords[4] - leftCoords[3], Game1.Textures["ovide_vignette"], screen => {
                ((GameScreen)screen).poets["Amants"].Call();}, true),

                new Button(leftOffset + cardWidth, rightCoords[4], cardWidth, leftCoords[5] - rightCoords[4], Game1.Textures["martial_vignette"], screen => {
                ((GameScreen)screen).poets["Peuple"].Call();}, true)
            }, screen => { ((GameScreen)screen).SpendActionPoints(1); }),

            //bienfaits
            new CollapseButton(leftOffset, Game1.HEIGHT / 6, cardWidth, (int)(cardWidth * ratio), Game1.Textures["card_bienfaits"], Game1.Textures["card_miracle_verso"], false, new List<Button>
            {
                new Button(leftOffset - betweenOffset, leftCoords[0], cardWidth, leftCoords[1] - leftCoords[0], Game1.Textures["nourriture"], screen => {
                    ((GameScreen)screen).SpendActionPoints(1);
                    ((GameScreen)screen).PlayRiteSound();
                    Gauges.IncrementGaugeValue("Peuple", 15, screen);
                    Gauges.IncrementGaugeValue("Senateurs", -5, screen);
                    Miracle.ActualizeMiracleChances();
                    System.Diagnostics.Debug.WriteLine("Distribution de nourriture");
                    Gauges.ShowGaugesValues(); }),

                new Button(leftOffset - betweenOffset, rightCoords[1], cardWidth, leftCoords[2] - rightCoords[1], Game1.Textures["processions religieuses"], screen => {
                    ((GameScreen)screen).SpendActionPoints(1);
                    ((GameScreen)screen).PlayRiteSound();
                    Gauges.IncrementGaugeValue("Senateurs", 15, screen);
                    Gauges.IncrementGaugeValue("Philosophes", -5, screen);
                    Miracle.ActualizeMiracleChances();
                    System.Diagnostics.Debug.WriteLine("Organisation des precessions religieuses");
                    Gauges.ShowGaugesValues(); }),

                new Button(leftOffset - betweenOffset, leftCoords[2], cardWidth, rightCoords[3] - rightCoords[2], Game1.Textures["theatre"], screen => {
                    ((GameScreen)screen).SpendActionPoints(1);
                    ((GameScreen)screen).PlayRiteSound();
                    Gauges.IncrementGaugeValue("Philosophes", 15, screen);
                    Gauges.IncrementGaugeValue("Militaires", -5, screen);
                    Miracle.ActualizeMiracleChances();
                    System.Diagnostics.Debug.WriteLine("Théâtre");
                    Gauges.ShowGaugesValues(); }),

                new Button(leftOffset - betweenOffset, leftCoords[3], cardWidth, leftCoords[4] - leftCoords[3], Game1.Textures["gladiateurs"], screen => {
                    ((GameScreen)screen).SpendActionPoints(1);
                    ((GameScreen)screen).PlayRiteSound();
                    Gauges.IncrementGaugeValue("Militaires", 15, screen);
                    Gauges.IncrementGaugeValue("Amants", -5, screen);
                    Miracle.ActualizeMiracleChances();
                    System.Diagnostics.Debug.WriteLine("Fabrication d'icônes");
                    Gauges.ShowGaugesValues(); }),

                new Button(leftOffset - betweenOffset, rightCoords[4], cardWidth, leftCoords[5] - rightCoords[4], Game1.Textures["fabriquer_icones"], screen => {
                    ((GameScreen)screen).SpendActionPoints(1);
                    ((GameScreen)screen).PlayRiteSound();
                    Gauges.IncrementGaugeValue("Amants", 15, screen);
                    Gauges.IncrementGaugeValue("Peuple", -5, screen);
                    System.Diagnostics.Debug.WriteLine("Combats de gladiateurs");
                    Gauges.ShowGaugesValues(); })
            }, screen => { }, true)
        };

        public CollapseButton Focused { get; set; } = null;

        public override void Init(Game1 game)
        {
            base.Init(game);

            readIntro = false;

            //Afficher l'intro

            poets = new Dictionary<string, Poet>();
            List<Poet> tempPoets = JsonConvert.DeserializeObject<List<Poet>>(File.ReadAllText("Content\\Design\\poets.json"));
            foreach (Poet p in tempPoets)
            {
                poets.Add(p.GaugeName, p);
            }

            Gauges.InitializeGauges(new List<string>(poets.Keys));
            foreach (Poet poet in poets.Values)
            {
                poet.Init();
            }

            MediaPlayer.Play(Game1.Musics["M_Egypte"]);
            MediaPlayer.IsRepeating = true;
            //MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

        }

        public void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            if (!Gauges.gameEnd)
            {
                //if (currentEvent == 0)
                //{
                //    MediaPlayer.Play(Game1.Musics["M_Egypte"]);
                //}
                if (currentEvent == 1)
                {
                    MediaPlayer.Play(Game1.Musics["M_Romain1"]);
                    MediaPlayer.IsRepeating = true;
                }
                //else if (currentEvent == 4)
                //{
                //    MediaPlayer.Play(Game1.Musics["M_Romain2"]);
                //}
                //else if (currentEvent == 5)
                //{
                //    MediaPlayer.Play(Game1.Musics["M_Romain3"]);
                //}
                //else if (currentEvent == 6)
                //{
                //    MediaPlayer.Play(Game1.Musics["M_Romain4"]);
                //}
            }
        }


        public void PlayRiteSound()
        {
            List<string> keys = new List<string>() { "Rite1", "Rite2", "Rite3" };
            Game1.Sounds[keys[Utils.Dice.GetRandint(0, 2)]].Play();

        }

        public bool SpendActionPoints(int amount, bool freeze = false)
        {
            int res = Timeline.SpendActionPoints(amount, freeze, this);

            if (res < 0) return false;

            if (res == 0 && !freeze && amount >= 0)
            {
                newDay = true;
            }
            if (res < 2) buttons[0].LockButton();

            return true;
        }

        public void NewDay()
        {
            ResetButtons();
            Focused = null;

            Miracle.ActualizeMiracleChances();

            eventDay = Timeline.TodayHasEvent();
            if (eventDay)
            {
                Timeline.CallEvent(this);
                currentEvent += 1;

                if (currentEvent == 1)
                {
                    MediaPlayer.Play(Game1.Musics["M_Transition"]);
                    MediaPlayer.IsRepeating = false;
                }
                else if (currentEvent == 4)
                {
                    MediaPlayer.Play(Game1.Musics["M_Romain2"]);
                    MediaPlayer.IsRepeating = true;
                }
                else if (currentEvent == 5)
                {
                    MediaPlayer.Play(Game1.Musics["M_Romain3"]);
                    MediaPlayer.IsRepeating = true;
                }
                else if (currentEvent == 6)
                {
                    MediaPlayer.Play(Game1.Musics["M_Romain4"]);
                    MediaPlayer.IsRepeating = true;
                }

                MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
                Game1.Sounds["Evenement"].Play();

                if (Timeline.miracleCurrentDelay > 0)
                {
                    buttons[1].LockButton();
                }

            }
            else
            {
                Gauges.NaturalDecay();
                Timeline.DecayMiracleDelay();

                //verrouillage du bouton de miracle si le délai n'est pas écoulé
                if(Timeline.miracleCurrentDelay > 0)
                {
                    buttons[1].LockButton();
                }

                string lowest = Gauges.GetLowestGauge();
                poets[lowest].Call();
            }
            
        }

        public void ResetButtons()
        {
            foreach (Button b in buttons)
            {
                b.Reset();
            }
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            if (readIntro)
            {
                if (miracle)
                {
                    curLetter += (int)Math.Ceiling(letterRate * gameTime.ElapsedGameTime.TotalSeconds);
                }
                if (miracle && mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    miracle = false;
                    miracleSuccess = false;
                    newDay = true;
                }

                if (helpRectangle.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    help = !help;
                }

                if (!help)
                {
                    pulsingActions = 0;

                    if (!eventDay && Focused == null)
                    {
                        int i = 0;
                        foreach (Button b in buttons)
                        {
                            b.Update(gameTime, prevMouseState, mouseState, this);
                            if (b.Hovered)
                            {
                                switch (i)
                                {
                                    case 0:
                                        pulsingActions += 2; //oracle
                                        break;
                                    case 1:
                                        pulsingActions += 5; //miracle
                                        break;
                                    case 2:
                                        pulsingActions += 1; //poetes
                                        break;
                                    case 3:
                                        pulsingActions += 0; //bienfaits
                                        break;
                                    default:
                                        break;
                                }
                            }
                            i++;
                        }
                    }
                    else if (Focused != null)
                    {
                        Focused.Update(gameTime, prevMouseState, mouseState, this);
                        if (Focused == buttons[3]) //bienfaits
                        {
                            pulsingActions += Focused.GetHovered();
                        }
                    }

                    foreach (Poet poet in poets.Values)
                    {
                        poet.Update(gameTime, mouseState);
                    }

                    bool endEvent = Timeline.Update(gameTime, mouseState, prevMouseState, this);
                    if (endEvent)
                    {
                        eventDay = false;
                        newDay = true;
                    }

                    if (newDay && (Focused == null || !Focused.FreezesTime))
                    {
                        newDay = false;
                        NewDay();
                    }
                }
            }
            else if(mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                readIntro = true;
            }

            prevMouseState = mouseState;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Texture2D bg = Game1.Textures["fond"];
            spriteBatch.Draw(bg, new Rectangle(0, 0, Game1.WIDTH, Game1.HEIGHT), Color.White);

            foreach (Button b in buttons)
            {
                if (b != Focused) b.Draw(gameTime, spriteBatch);
            }

            Timeline.Draw(spriteBatch, pulsingActions);

            if (Timeline.miracleCurrentDelay <= 0)
            {
                //Affichage de la chance de miracle
                int chance = Miracle.GetCurrentMiracleChance() + Timeline.GetLeftActionPoints() * Miracle.gainedMiracleChanceWithLowSatisfaction;
                string text = "Chance de miracle : " + chance + "%";
                List<string> lines = Utils.TextWrap.Wrap(text, cardWidth, Game1.font);

                int y = Game1.HEIGHT / 2;
                int x = leftOffset + 2 * (betweenOffset + cardWidth);

                if (lines.Count <= 1) x += (int)Game1.font.MeasureString(text).X / 2;

                foreach (string line in lines)
                {
                    spriteBatch.DrawString(Game1.font, line, new Vector2(x, y), Color.White);
                    y += (int)Game1.font.MeasureString("l").Y + 5;
                }
            }
            else
            {
                //Affichage du cooldown de miracle
                int t = Timeline.miracleCurrentDelay;
                string text = "Utilisable dans " + t + " tours";
                List<string> lines = Utils.TextWrap.Wrap(text, cardWidth, Game1.font);

                int y = Game1.HEIGHT / 2;
                int x = leftOffset + 2 * (betweenOffset + cardWidth);

                if (lines.Count <= 1) x += (int)Game1.font.MeasureString(text).X / 2;

                foreach (string line in lines)
                {
                    spriteBatch.DrawString(Game1.font, line, new Vector2(x, y), Color.White);
                    y += (int)Game1.font.MeasureString("l").Y + 5;
                }
            }

            if (Focused != null)
            {
                //spriteBatch.Draw(Game1.semiTransp, new Rectangle(0, 0, Game1.WIDTH, Game1.HEIGHT), Color.Black);
                Focused.Draw(gameTime, spriteBatch);
            }

            foreach (Poet poet in poets.Values)
            {
                poet.Draw(gameTime, spriteBatch);
            }

            if (miracle)
            {
                DrawMiracleDialogue(spriteBatch);
            }

            if (help)
            {
                spriteBatch.Draw(Game1.Textures["tutorial_layout"], new Rectangle(0, 0, Game1.WIDTH, Game1.HEIGHT), Color.White);
            }
            spriteBatch.Draw(Game1.Textures["bouton_help"], helpRectangle, Color.White);

            if (!readIntro)
            {
                spriteBatch.Draw(Game1.Textures["pannel_texte_debut"], new Rectangle(0, 0, Game1.WIDTH, Game1.HEIGHT), Color.White);
                string introText = "Ovide raconte qu'en 204 av. JC, alors que les Romains etaient en train de perdre la guerre contre Carthage, les oracles demanderent l'arrivee d'une nouvelle deesse dans le pantheon romain : Cybele. Une flotte fut envoyee pour aller chercher la divinite en Phrygie. Comme le roi phrygien ne voulait pas laisser partir Cybele, celle-ci lui apparut et lui ordonna de la laisser partir. Un roi effraye plus tard, les navires romains voguaient à nouveau vers Rome, la deesse à leur bord. Mais le bateau qui la transportait resta bloque dans le Tibre, et il fallut encore un autre miracle pour que Cybele parvienne enfin à Rome. Une jeune vierge voulant prouver sa chastete tira à elle seule les cordages et debloqua le navire. La deesse entra enfin dans Rome et un temple lui fut installe sur le Palatin. Mais les rites et valeurs associes à Cybele suscitent des reactions tres variees chez les romains, allant de l'horreur à la fascination, en passant par l'indifference. C'est à vous maintenant de faire en sorte que Cybele puisse être acceptee comme tous les autres dieux du pantheon romain! Influencez judicieusement l'opinion publique de differentes parties de la societe romaine en sa faveur, et survivez aux evenements marquants qui viendront chambouler vos efforts.Si une partie de la population deteste trop Cybele, celle - ci risque bien de finir jetee aux lions.";
                
                List<string> introLines = Utils.TextWrap.Wrap(introText, introCardWidth, Game1.Fonts["CaviarDreams"]);

                int yIntro = Game1.HEIGHT / 2 - introHeightOffset;
                int xIntro = Game1.WIDTH / 2 - introWidthOffset;

                foreach (string line in introLines)
                {
                    spriteBatch.DrawString(Game1.Fonts["CaviarDreams"], line, new Vector2(xIntro, yIntro), Color.Black);
                    yIntro += (int)Game1.font.MeasureString("l").Y + 5;
                }
            }

        }

        public void DrawMiracleDialogue(SpriteBatch spriteBatch)
        {
            Texture2D bubble = Game1.Textures["stele_evenements"];
            float ratio = bubble.Height / (float)bubble.Width;

            int textW = Game1.WIDTH - 2 * leftOffset - cardWidth;
            int textH = (int)(ratio * textW);
            int y = Game1.HEIGHT - textH - leftOffset;

            spriteBatch.Draw(bubble, new Rectangle(leftOffset, y, textW, textH), Color.White);

            string text;
            if (miracleSuccess)
            {
                text = "Les dieux ont repondu a votre priere.";
            }
            else
            {
                text = "Les dieux sont restes silencieux.";
            }

            y += textH / 3;
            foreach (string line in Utils.TextWrap.Wrap(text, textW, Game1.font, curLetter))
            {
                spriteBatch.DrawString(Game1.font, line, new Vector2(leftOffset + textW / 20, y), Color.Black);
                y += (int)Game1.font.MeasureString("l").Y + 5;
            }
        }

        public void ChangeScreen(bool victory, string highestGauge = "")
        {
            EndScreen screen = new EndScreen(victory, highestGauge);
            screen.Init(gameInstance);

            gameInstance.ChangeScreen(screen);
        }
    }
}
