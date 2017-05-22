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
        /// <summary>
        /// Escena/Screen de uso actual del juego
        /// </summary>
        public static Screen currentScreen;
        /// <summary>
        /// Manejador para la carga de contenido en el juego
        /// </summary>
        public static ContentManager contentManager;
        /// <summary>
        /// Clase tipada con los ajustes del juego
        /// </summary>
        public static AppCfg Settings;
        /// <summary>
        /// Rectángulo que contiene los límites visibles de la pantalla de juego
        /// </summary>
        public static Rectangle ScreenRect { get; set; }
        /// <summary>
        /// Indica si el juego debe cerrarse
        /// </summary>
        public static bool DoExit { get; set; }

        /// <summary>
        /// Para realizar todas las tareas de dibujado del juego
        /// </summary>
        private SpriteBatch spriteBatch;
        /// <summary>
        /// Indica la interfaz gráfica sobre la que se dibujará
        /// </summary>
        private GraphicsDeviceManager graphics;

        /// <summary>
        /// Permite leer un archivo Json y serializarlo para obtener los ajustes del juego
        /// </summary>
        /// <seealso cref="AppCfg"/>
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

        /// <summary>
        /// Permite serializar la clase actual AppCfg en un archivo Json y guardarlo.
        /// </summary>
        /// <seealso cref="AppCfg"/>
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

        /// <summary>
        /// Inicializa una instancia de la clase principal del juego
        /// </summary>
        public GameMain()
        {
            graphics = new GraphicsDeviceManager(this);
            this.IsMouseVisible = true;
            this.Window.Title = "SideShooting";
            Content.RootDirectory = "Content";
            ReadSettings();
        }

        /// <summary>
        /// Permite al juego hacer cualquier inicialización que necesite antes de arrancar.
        /// </summary>
        protected override void Initialize()
        {
            currentScreen = new MenuScreen(Content, GraphicsDevice);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            Viewport v = graphics.GraphicsDevice.Viewport;
            ScreenRect = new Rectangle(0, 0, v.Width, v.Height);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent se llamará una vez al inicio y es el lugar donde inicializar Contenido
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent se llamará al cerrar el juego y es el sitio de limpiar el contenido
        /// cargado en caso de necesitarlo
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Permite al juego correr su lógica interna y actualizar los datos de juego,
        /// comprobar colisiones, pulsación de teclas o reproducir audio.
        /// </summary>
        /// <param name="gameTime">Provee de los valores de tiempo actuales.</param>
        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            if (DoExit)
                Exit();

            currentScreen.Update(gameTime, this.IsActive);

            base.Update(gameTime);
        }

        /// <summary>
        /// Aquí se dibujara el juego en sí.
        /// </summary>
        /// <param name="gameTime">Provee de los valores de tiempo actuales.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            currentScreen.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
