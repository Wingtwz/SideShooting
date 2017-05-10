using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProyectoDAM
{
    public class Protagonist
    {
        public Texture2D Sprite { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int CurrentAnimation { get; set; }
        public Rectangle[] SourceAnimations { get; set; }
        public Vector2 Location { get; set; }

        private int currentFrame;
        private int totalFrames;

        public Protagonist(Texture2D sprite)
        {
            this.Sprite = sprite;
            Rows = 1;
            Columns = 4;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            SourceAnimations = new Rectangle[1];
            SourceAnimations[0] = new Rectangle(15, 6, 64, 22);
            CurrentAnimation = 0;
            Location = new Vector2(20);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Location, SourceAnimations[CurrentAnimation], Color.White);
        }
    }
}
