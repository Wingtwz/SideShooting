using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ProyectoDAM.Screens
{
    public class GameScreen : Screen
    {
        private Texture2D spriteProt;
        private Texture2D map;

        public GameScreen(ContentManager content, GraphicsDevice graphics) : base (content, graphics)
        {
            this.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(map, new Vector2(0), Color.White);

            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            spriteProt = Content.Load<Texture2D>("Images/character");
            map = Content.Load<Texture2D>("Images/mapahierba");
        }
    }
}
