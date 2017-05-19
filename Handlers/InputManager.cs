using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProyectoDAM;
using ProyectoDAM.Screens;
using SideShooting.Elements;

namespace SideShooting
{
    public static class InputManager
    {
        private static KeyboardState kbOldState;
        private static MouseState mOldState;

        public static void Game(GameScreen gameScreen, GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();

            int moveStep = 1;

            if (kbState.IsKeyDown(Keys.S))
            {
                if (gameScreen.Player.CurrentAnimation != 0)
                {
                    gameScreen.Player.CurrentAnimation = 0;
                }
                gameScreen.Player.Location = new Vector2(gameScreen.Player.Location.X, gameScreen.Player.Location.Y + moveStep);
                gameScreen.Player.Update(gameTime);
            }

            if (kbState.IsKeyDown(Keys.D))
            {
                if (gameScreen.Player.CurrentAnimation != 1)
                {
                    gameScreen.Player.CurrentAnimation = 1;
                }
                gameScreen.Player.Location = new Vector2(gameScreen.Player.Location.X + moveStep, gameScreen.Player.Location.Y);
                gameScreen.Player.Update(gameTime);
            }

            if (kbState.IsKeyDown(Keys.W))
            {
                if (gameScreen.Player.CurrentAnimation != 2)
                {
                    gameScreen.Player.CurrentAnimation = 2;
                }
                gameScreen.Player.Location = new Vector2(gameScreen.Player.Location.X, gameScreen.Player.Location.Y - moveStep);
                gameScreen.Player.Update(gameTime);
            }

            if (kbState.IsKeyDown(Keys.A))
            {
                if (gameScreen.Player.CurrentAnimation != 3)
                {
                    gameScreen.Player.CurrentAnimation = 3;
                }
                gameScreen.Player.Location = new Vector2(gameScreen.Player.Location.X - moveStep, gameScreen.Player.Location.Y);
                gameScreen.Player.Update(gameTime);
            }

            if (kbState.GetPressedKeys().Length <= 0)
            {
                gameScreen.Player.CurrentFrame = 1;
            }

            MouseState mState = Mouse.GetState();

            if (mState.LeftButton == ButtonState.Pressed && mOldState.LeftButton == ButtonState.Released)
            {
                var projectile = new Projectile(gameScreen.ProjectileSprite, gameScreen.Player.Location, mState.Position);
                gameScreen.Projectiles.Add(projectile);
                gameScreen.Connection.SendProjectile(projectile);
                System.Diagnostics.Debug.WriteLine(gameScreen.Projectiles.Count);
            }

            mOldState = mState;
        }
    }
}
