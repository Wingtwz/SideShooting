using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SideShooting;

namespace ProyectoDAM.Screens
{
    public class GameScreen : Screen
    {
        private Texture2D spriteCharacter;
        private Texture2D map;
        private Character player;
        private ConnectionManager cManager;
        private float timeSinceLastTick = 0;

        public GameScreen(ContentManager content, GraphicsDevice graphics, ConnectionManager cManager) : base (content, graphics)
        {
            this.LoadContent();
            this.player = new Character(spriteCharacter);
            this.cManager = cManager;
            this.cManager.Character = new Character(spriteCharacter);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(map, new Vector2(0), Color.White);

            player.Draw(SpriteBatch);
            cManager.Character.Draw(SpriteBatch);

            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            InputManager.Game(gameTime, player);

            //20 ticks por segundo (cada 0.05s) en cuanto a la actualización de servidor
            timeSinceLastTick += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastTick > 0.05)
            {
                cManager.SendPosition(player.Location, player.CurrentAnimation, player.CurrentFrame);
                timeSinceLastTick = 0;
            }
        }

        public override void LoadContent()
        {
            spriteCharacter = Content.Load<Texture2D>("Images/character");
            map = Content.Load<Texture2D>("Images/mapahierba");
        }
    }
}
