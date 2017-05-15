using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProyectoDAM;

namespace SideShooting
{
    public static class InputManager
    {
        public static void Game(GameTime gameTime, Protagonist prot)
        {
            KeyboardState state = Keyboard.GetState();

            int moveStep = 1;

            if (state.IsKeyDown(Keys.S))
            {
                if (prot.CurrentAnimation != 0)
                {
                    prot.CurrentAnimation = 0;
                }
                prot.Location = new Vector2(prot.Location.X, prot.Location.Y + moveStep);
                prot.Update(gameTime);
            }

            if (state.IsKeyDown(Keys.D))
            {
                if (prot.CurrentAnimation != 1)
                {
                    prot.CurrentAnimation = 1;
                }
                prot.Location = new Vector2(prot.Location.X + moveStep, prot.Location.Y);
                prot.Update(gameTime);
            }

            if (state.IsKeyDown(Keys.W))
            {
                if (prot.CurrentAnimation != 2)
                {
                    prot.CurrentAnimation = 2;
                }
                prot.Location = new Vector2(prot.Location.X, prot.Location.Y - moveStep);
                prot.Update(gameTime);
            }

            if (state.IsKeyDown(Keys.A))
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
    }
}
