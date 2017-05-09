using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ProyectoDAM
{
    public abstract class Screen
    {
        public virtual void Draw(GameTime gameTime) { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void LoadContent(ContentManager content, GraphicsDevice graphicsDevice) { }

        public virtual void UnloadContent() { }
    }
}
