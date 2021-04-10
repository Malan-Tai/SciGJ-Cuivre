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
            font = Content.Load<SpriteFont>("defaultFont");

            Sounds.Add("Miracles", Content.Load<SoundEffect>("Miracles"));
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
