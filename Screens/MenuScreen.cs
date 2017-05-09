using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProyectoDAM.Screens;

namespace ProyectoDAM
{
    public class MenuScreen : Screen
    {
        Rectangle rectPlay;
        Color color = Color.LightGray;

        public MenuScreen(ContentManager content, GraphicsDevice graphicsDevice) : base(content, graphicsDevice)
        {
            rectPlay = new Rectangle(100, 100, 100, 30);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(color);

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                GameMain.currentScreen = new TestScreen(new ContentManager(Content.ServiceProvider, Content.RootDirectory), GraphicsDevice);
            }

            base.Update(gameTime);
        }
    }
}
