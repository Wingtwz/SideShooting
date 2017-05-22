﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProyectoDAM;

namespace SideShooting.Screens
{
    public class WaitingScreen : Screen
    {
        public ConnectionManager Connection { get; set; }

        public Rectangle disconnectRect;

        private Texture2D bgImage, messageImage;
        private SpriteFont titleFont, messageFont;
        private string textStatus;

        public WaitingScreen(ContentManager content, GraphicsDevice graphicsDevice, ConnectionManager connection) : base(content, graphicsDevice)
        {
            this.LoadContent();
            this.Connection = connection;

            disconnectRect = new Rectangle(500, 500, 320, 80);

            textStatus = "Conectado, esperando a otro jugador";
        }

        public override void LoadContent()
        {
            messageFont = Content.Load<SpriteFont>("Fonts/SaviorMessage");
            titleFont = Content.Load<SpriteFont>("Fonts/GoooolyTitle");
            bgImage = Content.Load<Texture2D>("Images/SideShooting");
            messageImage = Content.Load<Texture2D>("Images/message");
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            SpriteBatch.Begin();

            SpriteBatch.Draw(bgImage, new Vector2(0), Color.White);

            int x = 400, y = 400;
            SpriteBatch.Draw(messageImage, new Rectangle(x, y, messageImage.Width * 2, messageImage.Height * 2), Color.White);
            SpriteBatch.DrawString(messageFont, textStatus, new Vector2(x + 50, y + 30), Color.White);
            
            SpriteBatch.DrawString(titleFont, "Desconectar", new Vector2(disconnectRect.Left + 10, disconnectRect.Top + 5), Color.White);

            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime, bool gameActive)
        {
            if (gameActive)
            {
                InputManager.Waiting(this, gameTime);
            }
        }
    }
}
