using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            KeyboardState state = Keyboard.GetState();

            int moveStep = 1;

            if (state.IsKeyDown(Keys.Down))
            {
                if (prot.CurrentAnimation != 0)
                {
                    prot.CurrentAnimation = 0;
                }
                prot.Location = new Vector2(prot.Location.X, prot.Location.Y + moveStep);
                prot.Update(gameTime);
            }

            if (state.IsKeyDown(Keys.Right))
            {
                if (prot.CurrentAnimation != 1)
                {
                    prot.CurrentAnimation = 1;
                }
                prot.Location = new Vector2(prot.Location.X + moveStep, prot.Location.Y);
                prot.Update(gameTime);
            }

            if (state.IsKeyDown(Keys.Up))
            {
                if (prot.CurrentAnimation != 2)
                {
                    prot.CurrentAnimation = 2;
                }
                prot.Location = new Vector2(prot.Location.X, prot.Location.Y - moveStep);
                prot.Update(gameTime);
            }

            if (state.IsKeyDown(Keys.Left))
            {
                if (prot.CurrentAnimation != 3)
                {
                    prot.CurrentAnimation = 3;
                }
                prot.Location = new Vector2(prot.Location.X - moveStep, prot.Location.Y);
                prot.Update(gameTime);
            }

            if (state.GetPressedKeys().Length <= 0)
            {
                prot.CurrentFrame = 1;
            }
        }

        public override void LoadContent()
        {
            spriteProt = Content.Load<Texture2D>("Images/character");
            map = Content.Load<Texture2D>("Images/mapahierba");
        }
    }
}
