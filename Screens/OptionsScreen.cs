using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SideShooting.Handlers;
using System;
using Microsoft.Xna.Framework.Media;

namespace SideShooting.Screens
{
    /// <summary>
    /// Contiene los métodos y parámetros de la escena con el menú de opciones del juego
    /// </summary>
    public class OptionsScreen : Screen
    {
        /// <summary>
        /// Contiene los límites para pulsar "Atrás" y retroceder a <see cref="MenuScreen"/>
        /// </summary>
        public Rectangle backRect;
        /// <summary>
        /// Contiene los límites de las zonas clicables de las opciones
        /// </summary>
        public Rectangle[] optionsRect = new Rectangle[2];

        /// <summary>
        /// Texturas de la imagen de fondo (bg), interfaz (ui) y textura para volcar información
        /// </summary>
        /// <remarks>dataTexture se usa para dibujar marcas rojas sobre las opciones activadas,
        /// se necesita volcar cierta información ahí para poder dibujar una línea.</remarks>
        private Texture2D bgImage, uiImage, dataTexture;

        /// <summary>
        /// Fuente de texto para dibujar
        /// </summary>
        private SpriteFont titleFont;
        /// <summary>
        /// Texto con las opciones del menú
        /// </summary>
        private string[] optionsText = { "Activar musica", "Activar sonidos" };
        /// <summary>
        /// Localización de las opciones del menú
        /// </summary>
        private Vector2[] optionsLocation = new Vector2[2];
        /// <summary>
        /// Canción del menú de juego
        /// </summary>
        private Song bgSong;

        /// <summary>
        /// Inicializa una instancia de la clase
        /// </summary>
        /// <param name="content">Gestor de contenido en uso</param>
        /// <param name="graphicsDevice">Interfaz sobre la que dibujar en uso</param>
        public OptionsScreen(ContentManager content, GraphicsDevice graphicsDevice) : base(content, graphicsDevice)
        {
            this.LoadContent();

            backRect = new Rectangle(550, 500, 320, 80);

            dataTexture = new Texture2D(GraphicsDevice, 1, 1);
            dataTexture.SetData<Color>(new Color[] { Color.White });

            Rectangle sourceRect = new Rectangle(16, 48, 24, 24);

            for (int i = 0, x = 170, y = 400; i < optionsText.Length; i++, x += 100)
            {
                optionsLocation[i] = new Vector2(x, y);
                x += 400;
                y += 20;
                optionsRect[i] = new Rectangle(x, y, sourceRect.Width * 2, sourceRect.Height * 2);
                y -= 20;
            }
        }

        /// <summary>
        /// Aquí se realizará la carga de contenido de archivos
        /// </summary>
        public override void LoadContent()
        {
            titleFont = Content.Load<SpriteFont>("Fonts/GoooolyTitle");
            bgImage = Content.Load<Texture2D>("Images/SideShooting");
            uiImage = Content.Load<Texture2D>("Images/ui");
            bgSong = Content.Load<Song>("Audio/menu");
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

            Rectangle sourceRect = new Rectangle(16, 48, 24, 24);

            for (int i = 0, spacing = 8; i < optionsText.Length; i++)
            {
                SpriteBatch.DrawString(titleFont, optionsText[i], optionsLocation[i], Color.White);
                SpriteBatch.Draw(uiImage, optionsRect[i], sourceRect, Color.White);
                if (i == 0? GameMain.Settings.MusicEnabled : GameMain.Settings.SoundEnabled)
                {
                    DrawLine(SpriteBatch, new Vector2(optionsRect[i].X + spacing, optionsRect[i].Y + spacing),
                        new Vector2((optionsRect[i].X + sourceRect.Width * 2) - spacing, (optionsRect[i].Y + sourceRect.Height * 2) - spacing));
                    DrawLine(SpriteBatch, new Vector2(optionsRect[i].X + spacing, (optionsRect[i].Y + sourceRect.Height * 2) - spacing),
                        new Vector2((optionsRect[i].X + sourceRect.Width * 2) - spacing, optionsRect[i].Y + spacing));
                }
            }

            SpriteBatch.DrawString(titleFont, "Atras", new Vector2(backRect.Left + 10, backRect.Top + 5), Color.White);

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
                InputManager.Options(this, gameTime, bgSong);
            }
        }

        /// <summary>
        /// Método para dibujar una línea
        /// </summary>
        /// <param name="sb"><see cref="SpriteBatch"/> en uso</param>
        /// <param name="start">Vector con el inicio de la línea</param>
        /// <param name="end">Vector con el final de la línea</param>
        public void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            sb.Draw(dataTexture, new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 3), null, Color.Red, angle, new Vector2(0, 0), SpriteEffects.None, 0);
        }
    }
}
