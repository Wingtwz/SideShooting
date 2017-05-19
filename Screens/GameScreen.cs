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
        public List<Projectile> EnemyProjectiles { get; set; }
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
            this.EnemyProjectiles = new List<Projectile>();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(map, new Vector2(0), Color.White);

            Player.Draw(SpriteBatch);
            Connection.Character.Draw(SpriteBatch);
            this.DrawProjectiles(Projectiles);
            this.DrawProjectiles(EnemyProjectiles);

            SpriteBatch.End();

            if (Player.DamageEffect % 2 != 0)
            {
                GraphicsDevice.Clear(Color.DarkRed);
            }
        }

        public override void Update(GameTime gameTime, bool gameActive)
        {
            if (gameActive)
            {
                InputManager.Game(this, gameTime);
            }

            this.UpdateProjectiles(Projectiles, gameTime);
            this.UpdateProjectiles(EnemyProjectiles, gameTime);

            if (Player.DamageEffect > 0)
            {
                Player.DamageEffect--;
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

        public void DrawProjectiles(List<Projectile> projectiles)
        {
            foreach (var p in projectiles)
            {
                p.Draw(SpriteBatch);
            }
        }

        public void UpdateProjectiles(List<Projectile> projectiles, GameTime gameTime)
        {
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Update(gameTime);

                if (!GameMain.ScreenRect.Contains(projectiles[i].Location.ToPoint()))
                {
                    Projectiles.Remove(projectiles[i]);
                }
            }
        }
    }
}
