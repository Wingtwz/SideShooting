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
        private SpriteFont testFont, titleFont;
        private Texture2D bgImage;
        private string textStatus = "Esto es un menu, pulsa aqui para continuar";
        private string[] menuText = { "Jugar", "Opciones", "Ayuda", "Creditos", "Salir" };
        private Rectangle[] menuRect;
        private Rectangle rectPlay = new Rectangle(230, 230, 100, 100);

        public MenuScreen(ContentManager content, GraphicsDevice graphicsDevice) : base(content, graphicsDevice)
        {
            this.LoadContent();

            menuRect = new Rectangle[5];
            for (int i = 0, x = 995, y = 250, width = 230, height = 80; i < menuRect.Length; i++, y += 90)
            {
                menuRect[i] = new Rectangle(x, y, width, height);
            }
        }

        public override void LoadContent()
        {
            testFont = Content.Load<SpriteFont>("Fonts/Fpsfont");
            titleFont = Content.Load<SpriteFont>("Fonts/GoooolyTitle");
            bgImage = Content.Load<Texture2D>("Images/SideShooting");
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            SpriteBatch.Begin();

            SpriteBatch.Draw(bgImage, new Vector2(0), Color.White);

            //for (int i = 0; i < menuRect.Length; i++)
            //    SpriteBatch.Draw(bgImage, menuRect[i], Color.White);

            //SpriteBatch.DrawString(testFont, textStatus, new Vector2(250), Color.Black);

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
                MouseState mouseState = Mouse.GetState();

                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (menuRect[0].Contains(mouseState.Position))
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
                    else if (menuRect[4].Contains(mouseState.Position))
                    {
                        GameMain.DoExit = true;
                    }
                }
            }
        }
    }
}
