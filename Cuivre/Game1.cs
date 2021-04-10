using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Cuivre.Code;
using Cuivre.Code.Screens;
using System.Collections.Generic;

namespace Cuivre
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Screen currentScreen;

        public static Texture2D white;
        public static Texture2D semiTransp;
        public static SpriteFont font;

        public static Dictionary<string, SoundEffect> Sounds { get; set; } = new Dictionary<string, SoundEffect>();
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            currentScreen = new GameScreen();
            currentScreen.Init(Content);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            white = Content.Load<Texture2D>("white");
            semiTransp = Content.Load<Texture2D>("semiTransp");
            font = Content.Load<SpriteFont>("defaultFont");

            Sounds.Add("Miracles", Content.Load<SoundEffect>("Miracles"));
            Sounds.Add("Evenement", Content.Load<SoundEffect>("Evenement"));
            Sounds.Add("MiracleRate", Content.Load<SoundEffect>("Miracle_rate"));
            Sounds.Add("Oracle", Content.Load<SoundEffect>("Oracle"));
            Sounds.Add("Poete1", Content.Load<SoundEffect>("Poete_1"));
            Sounds.Add("Poete2", Content.Load<SoundEffect>("Poete_2"));
            Sounds.Add("Poete3", Content.Load<SoundEffect>("Poete_3"));
            Sounds.Add("Poete4", Content.Load<SoundEffect>("Poete_4"));
            Sounds.Add("Rite1", Content.Load<SoundEffect>("Rite_1"));
            Sounds.Add("Rite2", Content.Load<SoundEffect>("Rite_2"));
            Sounds.Add("Rite3", Content.Load<SoundEffect>("Rite_3"));
            Sounds.Add("M_Egypte", Content.Load<SoundEffect>("M_Egypte"));
            Sounds.Add("M_Final", Content.Load<SoundEffect>("M_Final"));
            Sounds.Add("M_Romain1", Content.Load<SoundEffect>("M_Romain 1"));
            Sounds.Add("M_Romain2", Content.Load<SoundEffect>("M_Romain 2"));
            Sounds.Add("M_Romain3", Content.Load<SoundEffect>("M_Romain 3"));
            Sounds.Add("M_Romain4", Content.Load<SoundEffect>("M_Romain 4"));
            Sounds.Add("M_Transition", Content.Load<SoundEffect>("M_Transition 2 eg"));


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (currentScreen != null)
            {
                currentScreen.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            if (currentScreen != null)
            {
                currentScreen.Draw(gameTime, _spriteBatch);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
