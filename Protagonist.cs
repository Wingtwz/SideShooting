using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProyectoDAM
{
    public class Protagonist
    {
        public Texture2D Sprite { get; set; }
        public int CurrentAnimation { get; set; }
        public Rectangle[] SourceAnimations { get; set; }
        public Vector2 Location { get; set; }
        public int CurrentFrame { get; set; }

        private int spriteHeight = (int) (22 * 1.3);
        private int spriteWidth = (int)(15 * 1.3);
        private int maxFrames;
        private float timeSinceLastFrameStep = 0;

        public Protagonist(Texture2D sprite)
        {
            this.Sprite = sprite;
            CurrentFrame = 1;
            maxFrames = 3;

            SourceAnimations = new Rectangle[4];
            for (int i = 0, left = 1, top = 6, width = 64, height = 22; i < SourceAnimations.Length; i++, top += 32)
            {
                SourceAnimations[i] = new Rectangle(left, top, width, height);
            }

            SourceAnimations[0] = new Rectangle(1, 6, 64, 22);//separacion de 1px entre imagenes, 15*4 + 4 de ese px
            CurrentAnimation = 0;
            Location = new Vector2(20);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int width = SourceAnimations[CurrentAnimation].Width / (maxFrames + 1) - 1;

            Rectangle sourceRectangle = new Rectangle(SourceAnimations[CurrentAnimation].Left + width * (CurrentFrame + 1) + 1 * (CurrentFrame + 1),
                SourceAnimations[CurrentAnimation].Top, width, SourceAnimations[CurrentAnimation].Height);
            Rectangle destinationRectangle = new Rectangle((int)Location.X, (int)Location.Y, width, SourceAnimations[CurrentAnimation].Height);

            //spriteBatch.Draw(Sprite, Location, sourceRectangle, Color.White);
            spriteBatch.Draw(Sprite, new Rectangle((int)Location.X, (int)Location.Y, spriteWidth, spriteHeight), sourceRectangle, Color.White);
            //spriteBatch.Draw(Sprite, destinationRectangle, sourceRectangle, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            timeSinceLastFrameStep += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSinceLastFrameStep > 0.15)
            {
                CurrentFrame++;
                timeSinceLastFrameStep = 0;

                if (CurrentFrame == maxFrames)
                {
                    CurrentFrame = 0;
                }
            }
        }
    }
}
