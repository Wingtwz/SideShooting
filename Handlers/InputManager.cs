using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SideShooting.Elements;
using SideShooting.Screens;
using System.Net.Sockets;

namespace SideShooting.Handlers
{
    /// <summary>
    /// Contiene métodos auxiliares para gestionar las pulsaciones en las distintas escenas del juego
    /// </summary>
    public abstract class InputManager
    {
        /// <summary>
        /// Indica el último valor de estado que ha tenido el ratón
        /// </summary>
        private static MouseState mOldState;

        /// <summary>
        /// Gestiona cualquier pulsación para <see cref="CreditsScreen"/>
        /// </summary>
        /// <param name="creditsScreen">Escena de créditos que llama al método</param>
        /// <param name="gameTime">Valor de tiempo actual</param>
        public static void Credits(CreditsScreen creditsScreen, GameTime gameTime)
        {
            MouseState mState = Mouse.GetState();

            if (mState.LeftButton == ButtonState.Pressed && mOldState.LeftButton == ButtonState.Released)
            {
                if (creditsScreen.backRect.Contains(mState.Position))   //back
                {
                    GameMain.currentScreen = new MenuScreen(creditsScreen.Content, creditsScreen.GraphicsDevice);
                }
            }

            mOldState = mState;
        }

        /// <summary>
        /// Gestiona cualquier pulsación para <see cref="HelpScreen"/>
        /// </summary>
        /// <param name="helpScreen">Escena de ayuda que llama al método</param>
        /// <param name="gameTime">Valor de tiempo actual</param>
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

        /// <summary>
        /// Gestiona cualquier pulsación para <see cref="OptionsScreen"/>
        /// </summary>
        /// <param name="optionsScreen">Escena de opciones que llama al método</param>
        /// <param name="gameTime">Valor de tiempo actual</param>
        /// <param name="menuSong">Canción que debe sonar en el menú</param>
        public static void Options(OptionsScreen optionsScreen, GameTime gameTime, Song menuSong)
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
                    if (GameMain.Settings.MusicEnabled)
                    {
                        MediaPlayer.IsRepeating = true;
                        MediaPlayer.Play(menuSong);
                        GameMain.StartMusic = false;
                    }
                    else
                    {
                        MediaPlayer.Stop();
                    }
                }
                else if (optionsScreen.optionsRect[1].Contains(mState.Position))    //sound
                {
                    GameMain.Settings.SoundEnabled = !GameMain.Settings.SoundEnabled;
                }
            }

            mOldState = mState;
        }

        /// <summary>
        /// Gestiona cualquier pulsación para <see cref="MenuScreen"/>
        /// </summary>
        /// <param name="menuScreen">Escena de menú que llama al método</param>
        /// <param name="gameTime">Valor de tiempo actual</param>
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
                        bool? result = menuScreen.connection.Connect(GameMain.Settings.ServerIP, GameMain.Settings.Port);
                        if (result == true)
                        {
                            var game = new GameScreen(new ContentManager(menuScreen.Content.ServiceProvider, menuScreen.Content.RootDirectory), menuScreen.GraphicsDevice, menuScreen.connection);
                            game.Player.Location = new Vector2(1100, 350);
                            GameMain.currentScreen = game;
                        }
                        else if (result == false)
                        {
                            GameMain.currentScreen = new WaitingScreen(menuScreen.Content, menuScreen.GraphicsDevice, menuScreen.connection);
                        }
                        else
                        {
                            menuScreen.TextStatus = "IP no valida,\ncomprueba el archivo appsettings.json";
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

        /// <summary>
        /// Gestiona cualquier pulsación para <see cref="WaitingScreen"/>
        /// </summary>
        /// <param name="waitingScreen">Escena de espera que llama al método</param>
        /// <param name="gameTime">Valor de tiempo actual</param>
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

        /// <summary>
        /// Gestiona cualquier pulsación para <see cref="GameScreen"/>
        /// </summary>
        /// <param name="gameScreen">Escena de juego que llama al método</param>
        /// <param name="gameTime">Valor de tiempo actual</param>
        public static void Game(GameScreen gameScreen, GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();

            if (kbState.IsKeyDown(Keys.S))
            {
                if (gameScreen.Player.CurrentAnimation != 0)
                {
                    gameScreen.Player.CurrentAnimation = 0;
                }

                if (gameScreen.Player.Location.Y < 690)
                {
                    gameScreen.Player.Location = new Vector2(gameScreen.Player.Location.X, gameScreen.Player.Location.Y + gameScreen.Player.speed);
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
                    gameScreen.Player.Location = new Vector2(gameScreen.Player.Location.X + gameScreen.Player.speed, gameScreen.Player.Location.Y);
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
                    gameScreen.Player.Location = new Vector2(gameScreen.Player.Location.X, gameScreen.Player.Location.Y - gameScreen.Player.speed);
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
                    gameScreen.Player.Location = new Vector2(gameScreen.Player.Location.X - gameScreen.Player.speed, gameScreen.Player.Location.Y);
                }
                gameScreen.Player.Update(gameTime);
            }

            if (kbState.IsKeyDown(Keys.Q) && gameScreen.Player.BlueBar >= Character.MaxBlue)
            {
                gameScreen.Player.BlueBar = 0;
                gameScreen.Connection.SendCleaner();
                gameScreen.DoCleaner();
            }

            if (kbState.GetPressedKeys().Length <= 0)
            {
                gameScreen.Player.CurrentFrame = 1;
            }

            MouseState mState = Mouse.GetState();

            if (gameScreen.Player.ProjectilesAvailable > 0 && mState.LeftButton == ButtonState.Pressed && mOldState.LeftButton == ButtonState.Released)
            {
                gameScreen.Player.ProjectilesAvailable--;

                var projectile = new Projectile(gameScreen.ProjectileSprite, gameScreen.Player.Location, mState.Position);
                gameScreen.Projectiles.Add(projectile);
                gameScreen.Connection.SendProjectile(projectile);

                if (GameMain.Settings.SoundEnabled)
                {
                    gameScreen.shotSound.Play();
                }

                System.Diagnostics.Debug.WriteLine(gameScreen.Projectiles.Count);
            }

            if (gameScreen.Player.GreenBar >= Character.MaxGreen && mState.RightButton == ButtonState.Pressed)
            {
                gameScreen.Player.speed = 6;
            }

            mOldState = mState;
        }

        /// <summary>
        /// Se llama desde <see cref="GameScreen"/> si el juego ha terminado
        /// </summary>
        /// <returns>Indica si se debe salir del juego y volver al menú</returns>
        public static bool GameEnded()
        {
            KeyboardState kbState = Keyboard.GetState();
            bool result = false;

            if (kbState.IsKeyDown(Keys.Space))//kb.LeftButton == ButtonState.Pressed && mOldState.LeftButton == ButtonState.Released)
            {
                result = true;
            }

            return result;
        }
    }
}
