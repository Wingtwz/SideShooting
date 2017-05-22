using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SideShooting.Handlers;

namespace SideShooting.Screens
{
    /// <summary>
    /// Contiene los métodos y parámetros de la escena con el menú de espera del juego
    /// </summary>
    /// <remarks>Esta clase solo se usa cuando un jugador se conecta a un servidor sin más jugadores</remarks>
    public class WaitingScreen : Screen
    {
        /// <summary>
        /// Gestor de conexión ya conectado al servidor
        /// </summary>
        public ConnectionManager Connection { get; set; }

        /// <summary>
        /// Contiene los límites para pulsar "Desconectar" y retroceder a <see cref="MenuScreen"/>
        /// </summary>
        public Rectangle disconnectRect;

        /// <summary>
        /// Contiene las imagenes de fondo (bg) y sobre la que mostrar mensajes (message)
        /// </summary>
        private Texture2D bgImage, messageImage;
        /// <summary>
        /// Fuentes de texto para dibujar. Title: títulos. Message: mensajes menores.
        /// </summary>
        private SpriteFont titleFont, messageFont;
        /// <summary>
        /// Información con el estado actual de la espera
        /// </summary>
        private string textStatus;

        /// <summary>
        /// Crea una nueva instancia de la clase
        /// </summary>
        /// <param name="content">Gestor de contenido en uso</param>
        /// <param name="graphicsDevice">Interfaz sobre la que dibujar en uso</param>
        /// <param name="connection">Instancia de <see cref="ConnectionManager"/> con la que se está conectado al servidor</param>
        public WaitingScreen(ContentManager content, GraphicsDevice graphicsDevice, ConnectionManager connection) : base(content, graphicsDevice)
        {
            this.LoadContent();
            this.Connection = connection;

            disconnectRect = new Rectangle(500, 500, 320, 80);

            textStatus = "Conectado, esperando a otro jugador";
        }

        /// <summary>
        /// Aquí se realizará la carga de contenido de archivos
        /// </summary>
        public override void LoadContent()
        {
            messageFont = Content.Load<SpriteFont>("Fonts/SaviorMessage");
            titleFont = Content.Load<SpriteFont>("Fonts/GoooolyTitle");
            bgImage = Content.Load<Texture2D>("Images/SideShooting");
            messageImage = Content.Load<Texture2D>("Images/message");
        }

        /// <summary>
        /// Dibuja la escena actual y su información relevante
        /// </summary>
        /// <param name="gameTime">Valor de tiempo actual</param>
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            SpriteBatch.Begin();

            SpriteBatch.Draw(bgImage, new Vector2(0), Color.White);

            int x = 400, y = 400;
            SpriteBatch.Draw(messageImage, new Rectangle(x, y, messageImage.Width * 2, messageImage.Height * 2), Color.White);
            SpriteBatch.DrawString(messageFont, textStatus, new Vector2(x + 50, y + 30), Color.White);

            SpriteBatch.DrawString(titleFont, "Desconectar", new Vector2(disconnectRect.Left + 10, disconnectRect.Top + 5), Color.White);

            SpriteBatch.End();
        }

        /// <summary>
        /// Controla las pulsaciones de la escena
        /// </summary>
        /// <param name="gameTime">Valor de tiempo actual</param>
        /// <param name="gameActive">Indica si la ventana de juego tiene el foco en el sistema</param>
        public override void Update(GameTime gameTime, bool gameActive)
        {
            if (gameActive)
            {
                InputManager.Waiting(this, gameTime);
            }
        }
    }
}
