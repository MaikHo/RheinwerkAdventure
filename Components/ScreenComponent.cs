﻿using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using RheinwerkAdventure.Screens;
using Microsoft.Xna.Framework.Graphics;
using RheinwerkAdventure.Rendering;

namespace RheinwerkAdventure.Components
{
    /// <summary>
    /// Komponente zur Verwaltung von Screen-Overlays.
    /// </summary>
    internal class ScreenComponent : DrawableGameComponent
    {
        private readonly Stack<Screen> screens;

        private SpriteBatch spriteBatch;

        #region Shared Resources

        /// <summary>
        /// Ein einzelner Pixel
        /// </summary>
        public Texture2D Pixel { get; private set; }

        /// <summary>
        /// Standard-Schriftart für Dialoge
        /// </summary>
        public SpriteFont Font { get; private set; }

        /// <summary>
        /// Standard Hintergrund für Panels
        /// </summary>
        public NineTileRenderer Panel { get; private set; }

        /// <summary>
        /// Standard Hintergrund für Buttons
        /// </summary>
        public NineTileRenderer Button { get; private set; }

        /// <summary>
        /// Standard Hintergrund für selektierte Buttons.
        /// </summary>
        public NineTileRenderer ButtonHovered { get; private set; }

        /// <summary>
        /// Standard Hintergrund für einen Rahmen.
        /// </summary>
        public NineTileRenderer Border { get; private set; }

        #endregion

        /// <summary>
        /// Liefert den aktuellen Screen oder null zurück.
        /// </summary>
        public Screen ActiveScreen
        {
            get { return screens.Count > 0 ? screens.Peek() : null; }
        }

        /// <summary>
        /// Referenz auf das Game (Überschrieben mit spezialisiertem Type)
        /// </summary>
        public new RheinwerkGame Game { get; private set; }

        public ScreenComponent(RheinwerkGame game)
            : base(game)
        {
            Game = game;
            screens = new Stack<Screen>();
        }

        /// <summary>
        /// Zeigt den übergebenen Screen an.
        /// </summary>
        public void ShowScreen(Screen screen)
        {
            screens.Push(screen);
            screen.OnShow();
        }

        /// <summary>
        /// Entfernt den obersten Screen.
        /// </summary>
        public void CloseScreen()
        {
            if (screens.Count > 0)
            {
                var screen = screens.Pop();
                screen.OnHide();
            }
        }

        protected override void LoadContent()
        {
            // Sprite Batch erstellen
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Standard Pixel erstellen
            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData(new [] { Color.White });

            // Schriftart laden
            Font = Game.Content.Load<SpriteFont>("HudFont");

            // Hintergründe laden
            Texture2D texture = Game.Content.Load<Texture2D>("ui");
            Panel = new NineTileRenderer(texture, new Rectangle(190, 100, 100, 100), new Point(30, 30));
            Border = new NineTileRenderer(texture, new Rectangle(283, 200, 93, 94), new Point(30, 30));
            Button = new NineTileRenderer(texture, new Rectangle(0, 282, 190, 49), new Point(10, 10));
            ButtonHovered = new NineTileRenderer(texture, new Rectangle(0, 143, 190, 45), new Point(10, 10));
        }

        public override void Update(GameTime gameTime)
        {
            Screen activeScreen = ActiveScreen;
            if (activeScreen != null)
            {
                foreach (var control in activeScreen.Controls)
                    control.Update(gameTime);
                activeScreen.Update(gameTime);
                Game.Input.Handled = true;
            }

            // Spezialtasten prüfen
            if (!Game.Input.Handled)
            {
                if (Game.Input.Close)
                {
                    ShowScreen(new MainMenuScreen(this));
                    Game.Input.Handled = true;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.LinearWrap);
            foreach (var screen in screens)
                screen.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}
