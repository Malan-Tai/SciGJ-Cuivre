using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System.IO;

namespace Cuivre.Code.Screens
{
    class GameScreen : Screen
    {
        public Timeline Timeline { get; set; } = new Timeline();

        private List<Button> buttons = new List<Button>
        {
            //Pas très utile, mais je peux peut-être rajouter une sécurité si on ne trouve pas la clé dans le dico
            new Button(200, 100, 45, 45, Game1.white, screen => { 
                ((GameScreen)screen).SpendActionPoints(1);
                Gauges.IncrementGaugeValue("Peuple", 15);
                Gauges.IncrementGaugeValue("Senateurs", -15);
                System.Diagnostics.Debug.WriteLine("Distribution de nourriture");
                Gauges.ShowGaugesValues(); }),

            new Button(250, 100, 45, 45, Game1.white, screen => { 
                ((GameScreen)screen).SpendActionPoints(1);
                Gauges.IncrementGaugeValue("Senateurs", 15);
                Gauges.IncrementGaugeValue("Philosophes", -15);
                System.Diagnostics.Debug.WriteLine("Organisation des precessions religieuses");
                Gauges.ShowGaugesValues(); }),

            new Button(300, 100, 45, 45, Game1.white, screen => { 
                ((GameScreen)screen).SpendActionPoints(1);
                Gauges.IncrementGaugeValue("Philosophes", 15);
                Gauges.IncrementGaugeValue("Militaires", -15);
                System.Diagnostics.Debug.WriteLine("Théâtre");
                Gauges.ShowGaugesValues(); }),

            new Button(350, 100, 45, 45, Game1.white, screen => { 
                ((GameScreen)screen).SpendActionPoints(1);
                Gauges.IncrementGaugeValue("Amants", 15);
                Gauges.IncrementGaugeValue("Peuple", -15);
                System.Diagnostics.Debug.WriteLine("Fabrication d'icônes");
                Gauges.ShowGaugesValues(); }),

            new Button(400, 100, 45, 45, Game1.white, screen => { 
                ((GameScreen)screen).SpendActionPoints(1);
                Gauges.IncrementGaugeValue("Militaires", 15);
                Gauges.IncrementGaugeValue("Amants", -15);
                System.Diagnostics.Debug.WriteLine("Combats de gladiateurs");
                Gauges.ShowGaugesValues(); }),

            //Méthode de miracle appelée dans le SpendActionPoints pour tenir compte des PA
            new Button(200, 175, 50, 50, Game1.white, screen => {
                ((GameScreen)screen).SpendActionPoints(-1);
                System.Diagnostics.Debug.WriteLine("Chance de miracle augmentée : " + Miracle.GetCurrentMiracleChance() + "%"); }),

            //Bouton de l'oracle
            new Button(300, 175, 50, 50, Game1.white, screen => {
                ((GameScreen)screen).SpendActionPoints(2);
                //Display currentEventHint
                System.Diagnostics.Debug.WriteLine("L'oracle vous révèle une prophétie cheloue"); })
        };

        private Dictionary<string, Poet> poets;

        private bool newDay = false;

        public override void Init(ContentManager content)
        {
            base.Init(content);

            poets = new Dictionary<string, Poet>();
            List<Poet> tempPoets = JsonConvert.DeserializeObject<List<Poet>>(File.ReadAllText("Content\\Design\\poets.json"));
            foreach (Poet p in tempPoets)
            {
                poets.Add(p.GaugeName, p);
            }

            Gauges.InitializeGauges(new List<string>(poets.Keys));
            foreach (Poet poet in poets.Values)
            {
                poet.Init(content);
            }
        }

        public bool SpendActionPoints(int amount)
        {
            int res = Timeline.SpendActionPoints(amount);

            if (res < 0) return false;

            if (res == 0)
            {
                newDay = true;
            }

            return true;
        }

        public void NewDay()
        {
            ResetButtons();
            Gauges.NaturalDecay();

            string lowest = Gauges.GetLowestGauge();
            poets[lowest].Call();
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

            foreach (Poet poet in poets.Values)
            {
                poet.Update(gameTime, mouseState);
            }

            foreach (Button b in buttons)
            {
                b.Update(gameTime, prevMouseState, mouseState, this);
            }

            if (newDay)
            {
                newDay = false;
                NewDay();
            }

            prevMouseState = mouseState;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Poet poet in poets.Values)
            {
                poet.Draw(gameTime, spriteBatch);
            }

            foreach (Button b in buttons)
            {
                b.Draw(gameTime, spriteBatch);
            }

            Timeline.Draw(spriteBatch);
        }
    }
}
