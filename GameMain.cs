using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using SideShooting.Screens;
using System;
using System.IO;

namespace SideShooting
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameMain : Game
    {
        public static Screen currentScreen;
        public static ContentManager contentManager;
        public static AppCfg Settings;
        public static Rectangle ScreenRect { get; set; }
        public static bool DoExit { get; set; }

        SpriteBatch spriteBatch;
        GraphicsDeviceManager graphics;

        public void ReadSettings()
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader("appsettings.json");
                var json = sr.ReadToEnd();
                Settings = JsonConvert.DeserializeObject<AppCfg>(json);
            }
            catch (Exception ex) when (ex is IOException || ex is JsonReaderException)
            {
                Settings = new AppCfg();
                Settings.MusicEnabled = true;
                Settings.SoundEnabled = true;
                Settings.ServerIP = "127.0.0.1";
                Settings.Port = 31416;
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }

        public static void WriteSettings()
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter("appsettings.json");
                var json = JsonConvert.SerializeObject(Settings);
                sw.Write(json);
            }
            catch (IOException) { }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
        }

        public GameMain()
        {
            graphics = new GraphicsDeviceManager(this);
            this.IsMouseVisible = true;
            this.Window.Title = "SideShooting";
            Content.RootDirectory = "Content";
            ReadSettings();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            currentScreen = new MenuScreen(Content, GraphicsDevice);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            Viewport v = graphics.GraphicsDevice.Viewport;
            ScreenRect = new Rectangle(0, 0, v.Width, v.Height);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            if (DoExit)
                Exit();

            // TODO: Add your update logic here
            currentScreen.Update(gameTime, this.IsActive);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            currentScreen.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
