using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
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
        public static System.Drawing.Rectangle res = System.Windows.Forms.Screen.PrimaryScreen.Bounds;

        public static int WIDTH = (int)(res.Width * 0.92);
        public static int HEIGHT = (int)(res.Height * 0.92);

        public static Texture2D white;
        public static Texture2D semiTransp;
        public static SpriteFont font;

        public static Dictionary<string, SoundEffect> Sounds { get; set; } = new Dictionary<string, SoundEffect>();
        public static Dictionary<string, Song> Musics { get; set; } = new Dictionary<string, Song>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = WIDTH;
            _graphics.PreferredBackBufferHeight = HEIGHT;
            _graphics.ApplyChanges();
            _graphics.PreferredBackBufferWidth = WIDTH;
            _graphics.PreferredBackBufferHeight = HEIGHT;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            currentScreen = new MenuScreen();
            currentScreen.Init(Content, this);
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
            //Sounds.Add("Oracle", Content.Load<SoundEffect>("Oracle"));
            Sounds.Add("Poete1", Content.Load<SoundEffect>("Poete_1"));
            Sounds.Add("Poete2", Content.Load<SoundEffect>("Poete_2"));
            Sounds.Add("Poete3", Content.Load<SoundEffect>("Poete_3"));
            //Sounds.Add("Poete4", Content.Load<SoundEffect>("Poete_4"));
            Sounds.Add("Rite1", Content.Load<SoundEffect>("Rite_1"));
            Sounds.Add("Rite2", Content.Load<SoundEffect>("Rite_2"));
            Sounds.Add("Rite3", Content.Load<SoundEffect>("Rite_3"));

            Musics.Add("M_Egypte", Content.Load<Song>("M_Egypte"));
            Musics.Add("M_Final", Content.Load<Song>("M_Final"));
            Musics.Add("M_Romain1", Content.Load<Song>("M_Romain1"));
            Musics.Add("M_Romain2", Content.Load<Song>("M_Romain2"));
            Musics.Add("M_Romain3", Content.Load<Song>("M_Romain3"));
            Musics.Add("M_Romain4", Content.Load<Song>("M_Romain4"));
            Musics.Add("M_Transition", Content.Load<Song>("M_Transition"));

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

        public void ChangeScreen(Screen screen)
        {
            currentScreen = screen;
        }
    }
}
