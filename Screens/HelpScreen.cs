using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SideShooting.Handlers;

namespace SideShooting.Screens
{
    public class HelpScreen : Screen
    {
        public Rectangle backRect;

        private Texture2D bgImage;
        private SpriteFont titleFont, messageFont;
        private string message = "SideShooting es un juego en linea para dos jugadores...";

        public HelpScreen(ContentManager content, GraphicsDevice graphicsDevice) : base(content, graphicsDevice)
        {
            this.LoadContent();
            
            backRect = new Rectangle(500, 500, 320, 80);
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
            SpriteBatch.DrawString(messageFont, message, new Vector2(150, 320), Color.White);

            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime, bool gameActive)
        {
            if (gameActive)
            {
                InputManager.Help(this, gameTime);
            }
        }
    }
}
