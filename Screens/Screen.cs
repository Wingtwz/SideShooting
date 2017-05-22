using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SideShooting.Screens
{
    /// <summary>
    /// Clase padre para la herencia del resto de escenas
    /// </summary>
    public abstract class Screen
    {
        /// <summary>
        /// Gestor de contenido del juego
        /// </summary>
        public ContentManager Content { get; set; }
        /// <summary>
        /// Interfaz sobre la que dibujar del juego
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; set; }
        /// <summary>
        /// Herramienta de dibujado en uso por el juego
        /// </summary>
        public SpriteBatch SpriteBatch { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia con los datos proporcionados
        /// </summary>
        /// <param name="content">Gestor de contenido en uso</param>
        /// <param name="graphicsDevice">Interfaz sobre la que dibujar del juego</param>
        public Screen(ContentManager content, GraphicsDevice graphicsDevice)
        {
            Content = content;
            GraphicsDevice = graphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        //~Screen()
        //{
        //    Content.Unload();
        //}

        /// <summary>
        /// Aquí se realizara el dibujado de la escena
        /// </summary>
        /// <param name="gameTime">Valor actual de tiempo</param>
        public virtual void Draw(GameTime gameTime) { }

        /// <summary>
        /// Aquí se actualizará la lógica de la escena
        /// </summary>
        /// <param name="gameTime">Valor de tiempo actual</param>
        /// <param name="gameActive">Indica si la pantalla de juego tiene el foco del sistema</param>
        public virtual void Update(GameTime gameTime, bool gameActive) { }

        /// <summary>
        /// Aquí se realizará la carga de contenido desde archivos
        /// </summary>
        public virtual void LoadContent() { }
    }
}
