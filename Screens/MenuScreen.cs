using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SideShooting.Handlers;

namespace SideShooting.Screens
{
    /// <summary>
    /// Contiene los métodos y parámetros de la escena con el menú principal del juego
    /// </summary>
    public class MenuScreen : Screen
    {
        /// <summary>
        /// Texto con información de error o estado actual del juego
        /// </summary>
        public string TextStatus { get; set; }

        /// <summary>
        /// Gestor con el que se realiza la comunicación con el servidor
        /// </summary>
        public ConnectionManager connection;
        /// <summary>
        /// Delimita los rectángulos de los posibles menús del juego
        /// </summary>
        public Rectangle[] menuRect;
        //public Rectangle rectPlay = new Rectangle(230, 230, 100, 100);

        /// <summary>
        /// Canción del menú de juego
        /// </summary>
        private Song bgSong;
        /// <summary>
        /// Fuentes para dibujar en el juego. Title para títulos, message para mensajes menores.
        /// </summary>
        private SpriteFont messageFont, titleFont;
        /// <summary>
        /// Texturas que se usan en el menú. Bg: imagen de fondo. Message: imagen sobre la que mostrar mensajes.
        /// </summary>
        private Texture2D bgImage, messageImage;
        /// <summary>
        /// Texto de las opciones listadas en el menú
        /// </summary>
        private string[] menuText = { "Jugar", "Opciones", "Ayuda", "Creditos", "Salir" };

        /// <summary>
        /// Crea una instancia de la escena de menú
        /// </summary>
        /// <param name="content">Gestor de contenido en uso</param>
        /// <param name="graphicsDevice">Interfaz sobre la que dibujar en uso</param>
        public MenuScreen(ContentManager content, GraphicsDevice graphicsDevice) : base(content, graphicsDevice)
        {
            this.LoadContent();

            connection = new ConnectionManager();

            menuRect = new Rectangle[5];
            for (int i = 0, x = 995, y = 250, width = 230, height = 80; i < menuRect.Length; i++, y += 90)
            {
                menuRect[i] = new Rectangle(x, y, width, height);
            }

            if (GameMain.Settings.MusicEnabled && GameMain.StartMusic)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(bgSong);
                GameMain.StartMusic = false;
            }
        }

        /// <summary>
        /// Desde aquí se realiza la carga de contenido de archivos
        /// </summary>
        public override void LoadContent()
        {
            messageFont = Content.Load<SpriteFont>("Fonts/SaviorMessage");
            titleFont = Content.Load<SpriteFont>("Fonts/GoooolyTitle");
            bgImage = Content.Load<Texture2D>("Images/SideShooting");
            messageImage = Content.Load<Texture2D>("Images/message");
            bgSong = Content.Load<Song>("Audio/menu");
        }

        /// <summary>
        /// Desde aquí se dibuja el texto de las opciones del menú, la imagen de fondo y otra información
        /// </summary>
        /// <param name="gameTime">Valor de tiempo actual</param>
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            SpriteBatch.Begin();

            SpriteBatch.Draw(bgImage, new Vector2(0), Color.White);

            //for (int i = 0; i < menuRect.Length; i++)
            //    SpriteBatch.Draw(bgImage, menuRect[i], Color.White);

            if (TextStatus != null && TextStatus != "")
            {
                int x = 200, y = 400;
                SpriteBatch.Draw(messageImage, new Rectangle(x, y, messageImage.Width * 2, messageImage.Height * 2), Color.White);
                SpriteBatch.DrawString(messageFont, TextStatus, new Vector2(x + 50, y + 30), Color.White);
            }

            for (int i = 0, x = 1000, y = 250; i < menuText.Length; i++, y += 90)
            {
                SpriteBatch.DrawString(titleFont, menuText[i], new Vector2(x, y), Color.White);
            }

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Desde aquí se controlan las pulsaciones de la escena
        /// </summary>
        /// <param name="gameTime">Valor de tiempo actual</param>
        /// <param name="gameActive">Indica si la pantalla de juego tiene el foco del sistema</param>
        public override void Update(GameTime gameTime, bool gameActive)
        {
            if (gameActive)
            {
                InputManager.Menu(this, gameTime);
            }
        }
    }
}
