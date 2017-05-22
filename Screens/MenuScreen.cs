using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SideShooting.Handlers;

namespace SideShooting.Screens
{
    public class MenuScreen : Screen
    {
        public string TextStatus { get; set; }

        public ConnectionManager connection = new ConnectionManager();
        public Rectangle[] menuRect;
        //public Rectangle rectPlay = new Rectangle(230, 230, 100, 100);

        private Song bgSong;
        private SpriteFont messageFont, titleFont;
        private Texture2D bgImage, messageImage;
        private string[] menuText = { "Jugar", "Opciones", "Ayuda", "Creditos", "Salir" };

        public MenuScreen(ContentManager content, GraphicsDevice graphicsDevice) : base(content, graphicsDevice)
        {
            this.LoadContent();

            menuRect = new Rectangle[5];
            for (int i = 0, x = 995, y = 250, width = 230, height = 80; i < menuRect.Length; i++, y += 90)
            {
                menuRect[i] = new Rectangle(x, y, width, height);
            }

            if (GameMain.Settings.MusicEnabled)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(bgSong);
            }
        }

        public override void LoadContent()
        {
            messageFont = Content.Load<SpriteFont>("Fonts/SaviorMessage");
            titleFont = Content.Load<SpriteFont>("Fonts/GoooolyTitle");
            bgImage = Content.Load<Texture2D>("Images/SideShooting");
            messageImage = Content.Load<Texture2D>("Images/message");
            bgSong = Content.Load<Song>("Audio/menu");
        }

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

        public override void Update(GameTime gameTime, bool gameActive)
        {
            if (gameActive)
            {
                InputManager.Menu(this, gameTime);
            }
        }
    }
}
