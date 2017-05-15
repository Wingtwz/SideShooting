using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SideShooting;

namespace ProyectoDAM.Screens
{
    public class GameScreen : Screen
    {
        private Texture2D spriteProt;
        private Texture2D map;
        private Protagonist prot;
        private ConnectionManager cManager;
        private float timeSinceLastTick = 0;

        public GameScreen(ContentManager content, GraphicsDevice graphics, ConnectionManager cManager) : base (content, graphics)
        {
            this.LoadContent();
            this.prot = new Protagonist(spriteProt);
            this.cManager = cManager;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(map, new Vector2(0), Color.White);

            prot.Draw(SpriteBatch);

            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            InputManager.Game(gameTime, prot);

            //20 ticks por segundo (cada 0.05s) en cuanto a la actualización de servidor
            timeSinceLastTick += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastTick > 0.05)
            {
                cManager.SendPosition(prot.Location);
                timeSinceLastTick = 0;
            }
        }

        public override void LoadContent()
        {
            spriteProt = Content.Load<Texture2D>("Images/character");
            map = Content.Load<Texture2D>("Images/mapahierba");
        }
    }
}
