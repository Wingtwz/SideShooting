﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProyectoDAM
{
    public class MenuScreen : Screen
    {
        Rectangle rectPlay;

        public MenuScreen()
        {
            rectPlay = new Rectangle(100, 100, 100, 30);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            base.Draw(gameTime, spriteBatch, graphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}