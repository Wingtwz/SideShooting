using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SideShooting.Elements
{
    public class Projectile
    {
        public Vector2 Location { get; set; }
        public Vector2 Acceleration { get; set; }
        public int Speed { get; set; }
        public Texture2D Sprite { get; set; }
        public int CurrentFrame { get; set; }

        private float timeSinceLastFrameStep = 0;
        private int maxFrames = 4;

        public Projectile(Texture2D sprite, Vector2 playerLocation, Point mouseLocation)
        {
            this.Sprite = sprite;
            this.CurrentFrame = 0;
            this.Speed = 2;

            double atan2 = Math.Atan2(playerLocation.Y - mouseLocation.Y, playerLocation.X - mouseLocation.X);
            //double angle = (atan > 0 ? atan : (2 * Math.PI + atan)) * 360 / (2 * Math.PI);
            double radians = atan2 > 0 ? atan2 : 2 * Math.PI + atan2;

            this.Acceleration = new Vector2((float) -Math.Cos(radians) * Speed, (float) -Math.Sin(radians) * Speed);
            this.Location = playerLocation + this.Acceleration * 5;
        }

        public void Update(GameTime gameTime)
        {
            this.Location += this.Acceleration;
            //System.Diagnostics.Debug.WriteLine("Location " + this.Location);

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
