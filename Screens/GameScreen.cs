using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SideShooting;

namespace ProyectoDAM.Screens
{
    public class GameScreen : Screen
    {
        private Texture2D spriteProt;
        private Texture2D map;
        private Protagonist prot;

        public GameScreen(ContentManager content, GraphicsDevice graphics) : base (content, graphics)
        {
            this.LoadContent();
            prot = new Protagonist(spriteProt);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(map, new Vector2(0), Color.White);

            prot.Draw(SpriteBatch);

            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            InputManager.Game(gameTime, prot);
        }

        public override void LoadContent()
        {
            spriteProt = Content.Load<Texture2D>("Images/character");
            map = Content.Load<Texture2D>("Images/mapahierba");
        }
    }
}
