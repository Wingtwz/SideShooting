using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProyectoDAM.Screens;

namespace ProyectoDAM
{
    public class MenuScreen : Screen
    {
        private SpriteFont testFont;

        public MenuScreen(ContentManager content, GraphicsDevice graphicsDevice) : base(content, graphicsDevice)
        {
            this.LoadContent();
        }

        public override void LoadContent()
        {
            testFont = Content.Load<SpriteFont>("Fonts/Fpsfont");
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            SpriteBatch.Begin();

            SpriteBatch.DrawString(testFont, "Esto es un menu, pulsa para continuar", new Vector2(250), Color.Black);

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                ConnectionManager cManager = new ConnectionManager();
                cManager.Connect(GameMain.Settings.ServerIP, GameMain.Settings.Port);
                GameMain.currentScreen = new GameScreen(new ContentManager(Content.ServiceProvider, Content.RootDirectory), GraphicsDevice);
            }

            base.Update(gameTime);
        }
    }
}
