using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SideShooting.Handlers;

namespace SideShooting.Screens
{
    /// <summary>
    /// Escena que contiene los créditos del juego
    /// </summary>
    public class CreditsScreen : Screen
    {
        /// <summary>
        /// Contiene los límites para pulsar "Atrás" y retroceder a <see cref="MenuScreen"/>
        /// </summary>
        public Rectangle backRect;

        /// <summary>
        /// Textura con la imagen de fondo
        /// </summary>
        private Texture2D bgImage;
        /// <summary>
        /// Contiene las fuentes de texto a dibujar. Title para títulos y message para mensajes menores.
        /// </summary>
        private SpriteFont titleFont, messageFont;
        /// <summary>
        /// Texto con los créditos del juego
        /// </summary>
        private string message = "SideShooting por Carlos Carrera.\n" +
            "Interfaz de usuario por Buch. Sprites del personaje y casillas para el mapa por ArMM1998.\n" +
            "Pistas de musica de SUPER GAME MUSIC. Fuentes de texto de Aaron D. Chand y prask.\n" +
            "El resto de elementos, construccion del mapa, arte, sonidos y programacion hecho por Carlos Carrera.";

        /// <summary>
        /// Inicializa una instancia de la clase
        /// </summary>
        /// <param name="content">Gestor de contenido en uso</param>
        /// <param name="graphicsDevice">Interfaz sobre la que dibujar en uso</param>
        public CreditsScreen(ContentManager content, GraphicsDevice graphicsDevice) : base(content, graphicsDevice)
        {
            this.LoadContent();

            backRect = new Rectangle(550, 550, 320, 80);
        }

        /// <summary>
        /// Aquí se realizará la carga de contenido de archivos
        /// </summary>
        public override void LoadContent()
        {
            bgImage = Content.Load<Texture2D>("Images/SideShooting");
            messageFont = Content.Load<SpriteFont>("Fonts/SaviorMessage");
            titleFont = Content.Load<SpriteFont>("Fonts/GoooolyTitle");
        }

        /// <summary>
        /// Dibuja la escena actual y su información relevante
        /// </summary>
        /// <param name="gameTime">Valor de tiempo actual</param>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(bgImage, new Vector2(0), Color.White);

            SpriteBatch.DrawString(titleFont, "Atras", new Vector2(backRect.X, backRect.Y), Color.White);
            SpriteBatch.DrawString(messageFont, message, new Vector2(70, 350), Color.White);

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
                InputManager.Credits(this, gameTime);
            }
        }
    }
}
