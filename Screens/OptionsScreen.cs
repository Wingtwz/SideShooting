using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SideShooting.Handlers;
using System;

namespace SideShooting.Screens
{
    public class OptionsScreen : Screen
    {
        public Rectangle backRect;
        public Rectangle[] optionsRect = new Rectangle[2];

        private Texture2D bgImage, uiImage, dataTexture;
        private SpriteFont titleFont;
        private string[] optionsText = { "Activar musica", "Activar sonidos" };
        private Vector2[] optionsLocation = new Vector2[2];

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

        public override void LoadContent()
        {
            titleFont = Content.Load<SpriteFont>("Fonts/GoooolyTitle");
            bgImage = Content.Load<Texture2D>("Images/SideShooting");
            uiImage = Content.Load<Texture2D>("Images/ui");
        }

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

        public override void Update(GameTime gameTime, bool gameActive)
        {
            if (gameActive)
            {
                InputManager.Options(this, gameTime);
            }
        }

        public void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            sb.Draw(dataTexture, new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 3), null, Color.Red, angle, new Vector2(0, 0), SpriteEffects.None, 0);
        }
    }
}
