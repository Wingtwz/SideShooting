using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ProyectoDAM.Screens
{
    public class TestScreen : Screen
    {
        private Texture2D overworld;
        private SpriteFont fpsFont;
        private int score = 0;
        private Texture2D arrow;
        private float angle = 0;
        private Texture2D red;
        private Texture2D green;
        private Texture2D blue;

        private float blueAngle = 0;
        private float greenAngle = 0;
        private float redAngle = 0;

        private float blueSpeed = 0.025f;
        private float greenSpeed = 0.017f;
        private float redSpeed = 0.022f;

        private float distance = 100;

        private AnimatedSprite animatedSprite;

        public TestScreen(ContentManager content, GraphicsDevice graphicsDevice) : base(content, graphicsDevice)
        {
            this.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            SpriteBatch.DrawString(fpsFont, string.Format("FPS: {0:0.00}", frameRate), new Vector2(0, 0), Color.White);

            /*spriteBatch.Draw(overworld, new Rectangle(0, 0, 800, 480), Color.White);
            spriteBatch.Draw(character, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(100, 100), Color.White);*/

            Vector2 location = new Vector2(400, 240);
            Rectangle sourceRectangle = new Rectangle(0, 0, arrow.Width, arrow.Height);
            Vector2 origin = new Vector2(arrow.Width / 2, arrow.Height);

            //spriteBatch.Draw(arrow, location, sourceRectangle, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);

            Vector2 bluePosition = new Vector2(
                (float)Math.Cos(blueAngle) * distance,
                (float)Math.Sin(blueAngle) * distance);
            Vector2 greenPosition = new Vector2(
                            (float)Math.Cos(greenAngle) * distance,
                            (float)Math.Sin(greenAngle) * distance);
            Vector2 redPosition = new Vector2(
                            (float)Math.Cos(redAngle) * distance,
                            (float)Math.Sin(redAngle) * distance);

            Vector2 center = new Vector2(300, 140);

            SpriteBatch.Draw(blue, center + bluePosition, Color.White);
            SpriteBatch.Draw(green, center + greenPosition, Color.White);
            SpriteBatch.Draw(red, center + redPosition, Color.White);

            SpriteBatch.End();

            //animatedSprite.Draw(spriteBatch, new Vector2(400, 200));

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            score++;
            angle += 0.01f;
            animatedSprite.Update();

            blueAngle += blueSpeed;
            redAngle += redSpeed;
            greenAngle += greenSpeed;

            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            Texture2D character = Content.Load<Texture2D>("Images/SmileyWalk");
            animatedSprite = new AnimatedSprite(character, 4, 4);
            overworld = Content.Load<Texture2D>("Images/Overworld");
            fpsFont = Content.Load<SpriteFont>("Fonts/Fpsfont");
            arrow = Content.Load<Texture2D>("Images/arrow");
            red = Content.Load<Texture2D>("Images/red");
            green = Content.Load<Texture2D>("Images/green");
            blue = Content.Load<Texture2D>("Images/blue");

            base.LoadContent();
        }
    }
}
