using System.Net.Sockets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProyectoDAM.Screens;

namespace ProyectoDAM
{
    public class MenuScreen : Screen
    {
        private ConnectionManager connection = new ConnectionManager();
        private SpriteFont testFont;
        private string textStatus = "Esto es un menu, pulsa aqui para continuar";
        private Rectangle rectPlay = new Rectangle(230, 230, 100, 100);

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

            SpriteBatch.DrawString(testFont, textStatus, new Vector2(250), Color.Black);

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime, bool gameActive)
        {
            if (gameActive)
            {
                MouseState mouseState = Mouse.GetState();

                if (mouseState.LeftButton == ButtonState.Pressed && rectPlay.Contains(mouseState.Position))
                {
                    try
                    {
                        connection.Connect(GameMain.Settings.ServerIP, GameMain.Settings.Port);
                        GameMain.currentScreen = new GameScreen(new ContentManager(Content.ServiceProvider, Content.RootDirectory),
                            GraphicsDevice, connection);
                    }
                    catch (SocketException)
                    {
                        textStatus = "No se puede conectar al servidor"; //añadir log en caso de error?
                    }
                }
            }
        }
    }
}
