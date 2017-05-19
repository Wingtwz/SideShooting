using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SideShooting;
using SideShooting.Elements;

namespace ProyectoDAM.Screens
{
    public class GameScreen : Screen
    {
        public Character Player { get; set; }
        public Texture2D ProjectileSprite { get; set; }
        public List<Projectile> Projectiles { get; set; }
        public ConnectionManager Connection { get; set; }

        private Texture2D characterSprite;
        private Texture2D map;
        private float timeSinceLastTick = 0;

        public GameScreen(ContentManager content, GraphicsDevice graphics, ConnectionManager connection) : base (content, graphics)
        {
            this.LoadContent();
            this.Player = new Character(characterSprite);
            this.Connection = connection;
            this.Connection.Character = new Character(characterSprite);
            this.Connection.GameScreen = this;
            this.Projectiles = new List<Projectile>();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(map, new Vector2(0), Color.White);

            Player.Draw(SpriteBatch);
            Connection.Character.Draw(SpriteBatch);
            foreach (var p in Projectiles)
            {
                p.Draw(SpriteBatch);
            }

            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime, bool gameActive)
        {
            if (gameActive)
            {
                InputManager.Game(this, gameTime);
            }

            for (int i = Projectiles.Count-1; i >= 0; i--)
            {
                Projectiles[i].Update(gameTime);

                if (!GameMain.ScreenRect.Contains(Projectiles[i].Location.ToPoint()))
                {
                    Projectiles.Remove(Projectiles[i]);
                }
            }

            //20 ticks por segundo (cada 0.05s) en cuanto a la actualización de servidor
            timeSinceLastTick += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastTick > 0.05)
            {
                Connection.SendPosition(Player.Location, Player.CurrentAnimation, Player.CurrentFrame);
                timeSinceLastTick = 0;
            }
        }

        public override void LoadContent()
        {
            characterSprite = Content.Load<Texture2D>("Images/character");
            ProjectileSprite = Content.Load<Texture2D>("Images/proyectil");
            map = Content.Load<Texture2D>("Images/mapahierba");
        }
    }
}
