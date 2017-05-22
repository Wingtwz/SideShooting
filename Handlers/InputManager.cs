using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SideShooting.Elements;
using SideShooting.Screens;
using System.Net.Sockets;

namespace SideShooting.Handlers
{
    public abstract class InputManager
    {
        private static KeyboardState kbOldState;
        private static MouseState mOldState;

        public static void Credits(CreditsScreen helpScreen, GameTime gameTime)
        {
            MouseState mState = Mouse.GetState();

            if (mState.LeftButton == ButtonState.Pressed && mOldState.LeftButton == ButtonState.Released)
            {
                if (helpScreen.backRect.Contains(mState.Position))   //back
                {
                    GameMain.currentScreen = new MenuScreen(helpScreen.Content, helpScreen.GraphicsDevice);
                }
            }

            mOldState = mState;
        }

        public static void Help(HelpScreen helpScreen, GameTime gameTime)
        {
            MouseState mState = Mouse.GetState();

            if (mState.LeftButton == ButtonState.Pressed && mOldState.LeftButton == ButtonState.Released)
            {
                if (helpScreen.backRect.Contains(mState.Position))   //back
                {
                    GameMain.currentScreen = new MenuScreen(helpScreen.Content, helpScreen.GraphicsDevice);
                }
            }

            mOldState = mState;
        }

        public static void Options(OptionsScreen optionsScreen, GameTime gameTime)
        {
            MouseState mState = Mouse.GetState();

            if (mState.LeftButton == ButtonState.Pressed && mOldState.LeftButton == ButtonState.Released)
            {
                if (optionsScreen.backRect.Contains(mState.Position))   //back
                {
                    GameMain.WriteSettings();
                    GameMain.currentScreen = new MenuScreen(optionsScreen.Content, optionsScreen.GraphicsDevice);
                }
                else if (optionsScreen.optionsRect[0].Contains(mState.Position))    //music
                {
                    GameMain.Settings.MusicEnabled = !GameMain.Settings.MusicEnabled;
                    if (!GameMain.Settings.MusicEnabled)
                        MediaPlayer.Stop();
                }
                else if (optionsScreen.optionsRect[1].Contains(mState.Position))    //sound
                {
                    GameMain.Settings.SoundEnabled = !GameMain.Settings.SoundEnabled;
                }
            }

            mOldState = mState;
        }

        public static void Menu(MenuScreen menuScreen, GameTime gameTime)
        {
            MouseState mState = Mouse.GetState();

            if (mState.LeftButton == ButtonState.Pressed)
            {
                if (menuScreen.menuRect[0].Contains(mState.Position))
                {
                    menuScreen.TextStatus = null;

                    try
                    {
                        if (menuScreen.connection.Connect(GameMain.Settings.ServerIP, GameMain.Settings.Port))
                        {
                            var game = new GameScreen(new ContentManager(menuScreen.Content.ServiceProvider, menuScreen.Content.RootDirectory), menuScreen.GraphicsDevice, menuScreen.connection);
                            game.Player.Location = new Vector2(1100, 350);
                            GameMain.currentScreen = game;
                        }
                        else
                        {
                            GameMain.currentScreen = new WaitingScreen(menuScreen.Content, menuScreen.GraphicsDevice, menuScreen.connection);
                        }
                    }
                    catch (SocketException)
                    {
                        menuScreen.TextStatus = "No se puede conectar al servidor"; //añadir log en caso de error?
                    }
                }
                else if (menuScreen.menuRect[1].Contains(mState.Position))
                {
                    GameMain.currentScreen = new OptionsScreen(menuScreen.Content, menuScreen.GraphicsDevice);
                }
                else if (menuScreen.menuRect[2].Contains(mState.Position))
                {
                    GameMain.currentScreen = new HelpScreen(menuScreen.Content, menuScreen.GraphicsDevice);
                }
                else if (menuScreen.menuRect[3].Contains(mState.Position))
                {
                    GameMain.currentScreen = new CreditsScreen(menuScreen.Content, menuScreen.GraphicsDevice);
                }
                else if (menuScreen.menuRect[4].Contains(mState.Position))
                {
                    GameMain.DoExit = true;
                }
            }

            mOldState = mState;
        }

        public static void Waiting(WaitingScreen waitingScreen, GameTime gameTime)
        {
            MouseState mState = Mouse.GetState();

            if (mState.LeftButton == ButtonState.Pressed && waitingScreen.disconnectRect.Contains(mState.Position))
            {
                waitingScreen.Connection.Disconnect();
                GameMain.currentScreen = new MenuScreen(waitingScreen.Content, waitingScreen.GraphicsDevice);
            }

            mOldState = mState;
        }

        public static void Game(GameScreen gameScreen, GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();

            int moveStep = 2;

            if (kbState.IsKeyDown(Keys.S))
            {
                if (gameScreen.Player.CurrentAnimation != 0)
                {
                    gameScreen.Player.CurrentAnimation = 0;
                }

                if (gameScreen.Player.Location.Y < 690)
                {
                    gameScreen.Player.Location = new Vector2(gameScreen.Player.Location.X, gameScreen.Player.Location.Y + moveStep);
                }
                gameScreen.Player.Update(gameTime);
            }

            if (kbState.IsKeyDown(Keys.D))
            {
                if (gameScreen.Player.CurrentAnimation != 1)
                {
                    gameScreen.Player.CurrentAnimation = 1;
                }

                if (gameScreen.Player.Location.X < 1260)
                {
                    gameScreen.Player.Location = new Vector2(gameScreen.Player.Location.X + moveStep, gameScreen.Player.Location.Y);
                }
                gameScreen.Player.Update(gameTime);
            }

            if (kbState.IsKeyDown(Keys.W))
            {
                if (gameScreen.Player.CurrentAnimation != 2)
                {
                    gameScreen.Player.CurrentAnimation = 2;
                }

                if (gameScreen.Player.Location.Y > 0)
                {
                    gameScreen.Player.Location = new Vector2(gameScreen.Player.Location.X, gameScreen.Player.Location.Y - moveStep);
                }
                gameScreen.Player.Update(gameTime);
            }

            if (kbState.IsKeyDown(Keys.A))
            {
                if (gameScreen.Player.CurrentAnimation != 3)
                {
                    gameScreen.Player.CurrentAnimation = 3;
                }

                if (gameScreen.Player.Location.X > 0)
                {
                    gameScreen.Player.Location = new Vector2(gameScreen.Player.Location.X - moveStep, gameScreen.Player.Location.Y);
                }
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

        public static bool GameEnded()
        {
            MouseState mState = Mouse.GetState();

            if (mState.LeftButton == ButtonState.Pressed && mOldState.LeftButton == ButtonState.Released)
            {
                return true;
            }

            mOldState = mState;

            return false;
        }
    }
}
