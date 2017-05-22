using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SideShooting.Handlers;

namespace SideShooting.Screens
{
    /// <summary>
    /// Escena que contiene la ayuda del juego
    /// </summary>
    public class HelpScreen : Screen
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
        /// Texto con la ayuda del juego
        /// </summary>
        private string message = "SideShooting es un juego en linea para dos jugadores de vista lateral.\n" +
            "Una vez conectados dos jugadores, la partida comenzara.\n" +
            "El objetivo es derrotar al jugador acertandole con disparos, y al mismo tiempo esquivar los del rival.\n" +
            "Cada jugador puede aguantar hasta 6 disparos antes de perder.\n" +
            "Los controles son:\n" +
            "W/A/S/D: Moverse hacia arriba/izquierda/abajo/derecha/n" +
            "Clic izquierdo: disparar en direccion al cursor del raton.\n" +
            "Clic derecho: aceleron - permite moverse mas rapido durante un corto espacio de tiempo.\n" +
            "Q: fogueo - permite borrar todos los proyectiles que existen en el mapa.\n\n" +
            "Ten en cuenta que aceleron solo estara disponible si la barra verde esta al maximo, y fogueo si lo esta la barra azul.\n" +
            "Ademas, solo podras tener un maximo de 9 proyectiles a la vez en el mapa, el numero restante se indica en la interfaz.";

        /// <summary>
        /// Inicializa una instancia de la clase
        /// </summary>
        /// <param name="content">Gestor de contenido en uso</param>
        /// <param name="graphicsDevice">Interfaz sobre la que dibujar en uso</param>
        public HelpScreen(ContentManager content, GraphicsDevice graphicsDevice) : base(content, graphicsDevice)
        {
            this.LoadContent();

            backRect = new Rectangle(550, 620, 320, 80);
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
            SpriteBatch.DrawString(messageFont, message, new Vector2(70, 300), Color.White);

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
                InputManager.Help(this, gameTime);
            }
        }
    }
}
