﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SideShooting;

namespace ProyectoDAM.Screens
{
    public class GameScreen : Screen
    {
        public Character Player { get; set; }
        public Texture2D ProjectileSprite { get; set; }

        private Texture2D spriteCharacter;
        private Texture2D map;
        private ConnectionManager cManager;
        private float timeSinceLastTick = 0;

        public GameScreen(ContentManager content, GraphicsDevice graphics, ConnectionManager cManager) : base (content, graphics)
        {
            this.LoadContent();
            this.Player = new Character(spriteCharacter);
            this.cManager = cManager;
            this.cManager.Character = new Character(spriteCharacter);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(map, new Vector2(0), Color.White);

            Player.Draw(SpriteBatch);
            cManager.Character.Draw(SpriteBatch);

            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime, bool gameActive)
        {
            if (gameActive)
            {
                InputManager.Game(this, gameTime);
            }

            //20 ticks por segundo (cada 0.05s) en cuanto a la actualización de servidor
            timeSinceLastTick += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastTick > 0.05)
            {
                cManager.SendPosition(Player.Location, Player.CurrentAnimation, Player.CurrentFrame);
                timeSinceLastTick = 0;
            }
        }

        public override void LoadContent()
        {
            spriteCharacter = Content.Load<Texture2D>("Images/character");
            map = Content.Load<Texture2D>("Images/mapahierba");
        }
    }
}
