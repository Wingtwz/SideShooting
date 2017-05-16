using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ProyectoDAM
{
    public abstract class Screen
    {
        public ContentManager Content { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public SpriteBatch SpriteBatch { get; set; }

        public Screen(ContentManager content, GraphicsDevice graphicsDevice)
        {
            Content = content;
            GraphicsDevice = graphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        ~Screen()
        {
            Content.Unload();
        }

        public virtual void Draw(GameTime gameTime) { }

        public virtual void Update(GameTime gameTime, bool gameActive) { }

        public virtual void LoadContent() { }
    }
}
