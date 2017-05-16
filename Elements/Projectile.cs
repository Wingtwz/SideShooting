using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SideShooting.Elements
{
    public class Projectile
    {
        public Vector2 Location { get; set; }
        public Vector2 Acceleration { get; set; }
        public Texture2D Sprite { get; set; }
        public int CurrentFrame { get; set; }

        private float timeSinceLastFrameStep = 0;
        private int maxFrames = 4;

        public Projectile(Texture2D sprite, Vector2 playerLocation, Point mouseLocation)
        {
            this.Sprite = sprite;
            this.CurrentFrame = 0;

            double angle = Math.Atan2(playerLocation.Y - mouseLocation.Y, playerLocation.X - mouseLocation.X);
            System.Diagnostics.Debug.WriteLine(angle);
        }

        public void Update(GameTime gameTime)
        {
            timeSinceLastFrameStep += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSinceLastFrameStep > 0.15)
            {
                CurrentFrame++;
                timeSinceLastFrameStep = 0;

                if (CurrentFrame >= maxFrames)
                {
                    CurrentFrame = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int side = 20;//lo meto a piñon

            Rectangle sourceRectangle = new Rectangle(1, (1 * (CurrentFrame + 1) + side * CurrentFrame) + 1, side, side);

            spriteBatch.Draw(Sprite, Location, sourceRectangle, Color.White);
        }
    }
}
