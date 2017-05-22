using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SideShooting.Handlers;

namespace SideShooting.Screens
{
    public class CreditsScreen : Screen
    {
        public Rectangle backRect;

        private Texture2D bgImage;
        private SpriteFont titleFont, messageFont;
        private string message = "SideShooting por Carlos Carrera.\n" +
            "Interfaz de usuario por Buch. Sprites del personaje y casillas para el mapa por ArMM1998.\n" +
            "Pistas de musica de SUPER GAME MUSIC. Fuentes de texto de Aaron D. Chand y prask.\n" +
            "El resto de elementos, construccion del mapa, arte, sonidos y programacion hecho por Carlos Carrera.";

        public CreditsScreen(ContentManager content, GraphicsDevice graphicsDevice) : base(content, graphicsDevice)
        {
            this.LoadContent();

            backRect = new Rectangle(550, 550, 320, 80);
        }

        public override void LoadContent()
        {
            bgImage = Content.Load<Texture2D>("Images/SideShooting");
            messageFont = Content.Load<SpriteFont>("Fonts/SaviorMessage");
            titleFont = Content.Load<SpriteFont>("Fonts/GoooolyTitle");
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(bgImage, new Vector2(0), Color.White);

            SpriteBatch.DrawString(titleFont, "Atras", new Vector2(backRect.X, backRect.Y), Color.White);
            SpriteBatch.DrawString(messageFont, message, new Vector2(70, 350), Color.White);

            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime, bool gameActive)
        {
            if (gameActive)
            {
                InputManager.Credits(this, gameTime);
            }
        }
    }
}
