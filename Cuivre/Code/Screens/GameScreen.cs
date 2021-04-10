﻿using System;
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

        private Dictionary<string, Poet> poets;

        private bool newDay = false;
        private bool eventDay = false;


        private List<Button> buttons = new List<Button>
        {

            //Bouton de l'oracle
            new Button(25, 100, 150, 310, Game1.white, screen => {

            ((GameScreen)screen).SpendActionPoints(2);
            ((GameScreen)screen).SpendActionPoints(2);
            ((GameScreen)screen).Timeline.CallOracle();
            }),

            new CollapseButton(185, 100, 150, 310, Game1.white, new List<Button>
            {
                new Button(195, 110, 130, 50, Game1.white, screen => {
                ((GameScreen)screen).SpendActionPoints(1);
                Gauges.IncrementGaugeValue("Peuple", 15);
                Gauges.IncrementGaugeValue("Senateurs", -5);
                System.Diagnostics.Debug.WriteLine("Distribution de nourriture");
                Gauges.ShowGaugesValues(); }),

                new Button(195, 170, 130, 50, Game1.white, screen => {
                ((GameScreen)screen).SpendActionPoints(1);
                Gauges.IncrementGaugeValue("Senateurs", 15);
                Gauges.IncrementGaugeValue("Philosophes", -5);
                System.Diagnostics.Debug.WriteLine("Organisation des precessions religieuses");
                Gauges.ShowGaugesValues(); }),

                new Button(195, 230, 130, 50, Game1.white, screen => {
                ((GameScreen)screen).SpendActionPoints(1);
                Gauges.IncrementGaugeValue("Philosophes", 15);
                Gauges.IncrementGaugeValue("Militaires", -5);
                System.Diagnostics.Debug.WriteLine("Théâtre");
                Gauges.ShowGaugesValues(); }),

                new Button(195, 290, 130, 50, Game1.white, screen => {
                ((GameScreen)screen).SpendActionPoints(1);
                Gauges.IncrementGaugeValue("Amants", 15);
                Gauges.IncrementGaugeValue("Peuple", -5);
                System.Diagnostics.Debug.WriteLine("Fabrication d'icônes");
                Gauges.ShowGaugesValues(); }),

                new Button(195, 350, 130, 50, Game1.white, screen => {
                ((GameScreen)screen).SpendActionPoints(1);
                Gauges.IncrementGaugeValue("Militaires", 15);
                Gauges.IncrementGaugeValue("Amants", -5);
                System.Diagnostics.Debug.WriteLine("Combats de gladiateurs");
                Gauges.ShowGaugesValues(); })


            }),

            //Méthode de miracle appelée dans le SpendActionPoints pour tenir compte des PA
            new Button(345, 100, 150, 310, Game1.white, screen => {
                ((GameScreen)screen).SpendActionPoints(-1);
                System.Diagnostics.Debug.WriteLine("Chance de miracle augmentée : " + Miracle.GetCurrentMiracleChance() + "%"); }),

            new CollapseButton(505, 100, 150, 310, Game1.white, new List<Button>
            {
                new Button(515, 110, 130, 50, Game1.white, screen => {
                ((GameScreen)screen).SpendActionPoints(1);
                ((GameScreen)screen).poets["Peuple"].Call();
                System.Diagnostics.Debug.WriteLine("On demande son avis au poète du Peuple");}),

                new Button(515, 170, 130, 50, Game1.white, screen => {
                ((GameScreen)screen).SpendActionPoints(1);
                ((GameScreen)screen).poets["Senateurs"].Call();
                System.Diagnostics.Debug.WriteLine("On demande son avis au poète des Sénateurs");}),

                new Button(515, 230, 130, 50, Game1.white, screen => {
                ((GameScreen)screen).SpendActionPoints(1);
                ((GameScreen)screen).poets["Philosophes"].Call();
                System.Diagnostics.Debug.WriteLine("On demande son avis au poète des Philosophes");}),

                new Button(515, 290, 130, 50, Game1.white, screen => {
                ((GameScreen)screen).SpendActionPoints(1);
                ((GameScreen)screen).poets["Amants"].Call();
                System.Diagnostics.Debug.WriteLine("On demande son avis au poète des Amants");}),

                new Button(515, 350, 130, 50, Game1.white, screen => {
                ((GameScreen)screen).SpendActionPoints(1);
                ((GameScreen)screen).poets["Militaires"].Call();
                System.Diagnostics.Debug.WriteLine("On demande son avis au poète des Militaires");}),


            })

        };

        

        public override void Init(ContentManager content)
        {
            base.Init(content);

            //EventPool.AddEvents();

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

            eventDay = Timeline.TodayHasEvent();
            if (eventDay)
            {
                Timeline.CallEvent();
            }
            else
            {
                Gauges.NaturalDecay();
                Timeline.DecayMiracleDelay();

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

            foreach (Poet poet in poets.Values)
            {
                poet.Update(gameTime, mouseState);
            }

            foreach (Button b in buttons)
            {
                b.Update(gameTime, prevMouseState, mouseState, this);
            }

            double eventDelay = Timeline.Update(gameTime, mouseState);
            if (eventDay && eventDelay <= 0 && mouseState.LeftButton == ButtonState.Pressed)
            {
                eventDay = false;
                newDay = true;
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
            foreach (Button b in buttons)
            {
                b.Draw(gameTime, spriteBatch);
            }

            foreach (Poet poet in poets.Values)
            {
                poet.Draw(gameTime, spriteBatch);
            }

            Timeline.Draw(spriteBatch);
        }
    }
}
