using Microsoft.Xna.Framework.Graphics;

namespace ProyectoDAM
{
    public class Protagonist
    {
        public Texture2D Sprite { get; set; }

        public Protagonist(Texture2D sprite)
        {
            this.Sprite = sprite;
        }
    }
}
