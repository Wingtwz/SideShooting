using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SideShooting.Elements;
using SideShooting.Handlers;
using System.Collections.Generic;
using System.IO;

namespace SideShooting.Screens
{
    public class GameScreen : Screen
    {
        public Character Player { get; set; }
        public Texture2D ProjectileSprite { get; set; }
        public List<Projectile> Projectiles { get; set; }
        public List<Projectile> EnemyProjectiles { get; set; }
        public ConnectionManager Connection { get; set; }

        public SoundEffect hitSound, shotSound;

        private Texture2D characterSprite;
        private Texture2D map;
        private Texture2D clouds;
        private Vector2[] cloudVectors = new Vector2[4];
        private Song gameSong;
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

            for (int i = 0, x = 0, y = -720; i < cloudVectors.Length; i += 2, y += 720)
            {
                cloudVectors[i] = new Vector2(x, y);
                cloudVectors[i + 1] = new Vector2(x + 1280, y);
            }

            if (GameMain.Settings.MusicEnabled)
            {
                MediaPlayer.Play(gameSong);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(map, new Vector2(0), Color.White);

            Player.Draw(SpriteBatch);
            Connection.Character.Draw(SpriteBatch);
            this.DrawProjectiles(Projectiles);
            this.DrawProjectiles(EnemyProjectiles);

            foreach (var vector in cloudVectors)
            {
                SpriteBatch.Draw(clouds, vector, Color.White);
            }

            SpriteBatch.End();

            if (Player.DamageEffect % 2 != 0)
            {
                GraphicsDevice.Clear(Color.DarkRed);
            }
        }

        public override void Update(GameTime gameTime, bool gameActive)
        {
            try
            {
                if (Connection.ConnectionAlive)
                {
                    if (gameActive)
                    {
                        InputManager.Game(this, gameTime);
                    }

                    this.UpdateProjectiles(Projectiles, gameTime);
                    this.UpdateProjectiles(EnemyProjectiles, gameTime);

                    this.CheckCollisions(EnemyProjectiles);

                    for (int i = 0; i < cloudVectors.Length; i++)
                    {
                        cloudVectors[i] = new Vector2(cloudVectors[i].X < -1279 ? 1280 : cloudVectors[i].X - 1, cloudVectors[i].Y > 720 ? -720 : cloudVectors[i].Y + 1);
                    }

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
                else
                {
                    if (InputManager.GameEnded())
                    {
                        GameMain.currentScreen = new MenuScreen(Content, GraphicsDevice);
                    }
                }
            }
            catch (IOException)
            {
                var menu = new MenuScreen(Content, GraphicsDevice);
                menu.TextStatus = "Error de conexion con el servidor";
                GameMain.currentScreen = menu;
            }
        }

        public override void LoadContent()
        {
            characterSprite = Content.Load<Texture2D>("Images/character");
            ProjectileSprite = Content.Load<Texture2D>("Images/proyectil");
            map = Content.Load<Texture2D>("Images/mapahierba");
            gameSong = Content.Load<Song>("Audio/juego2");
            hitSound = Content.Load<SoundEffect>("Audio/enemigotocado");
            shotSound = Content.Load<SoundEffect>("Audio/dmgrecibido");
            clouds = Content.Load<Texture2D>("Images/capanubes");
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

        public void CheckCollisions(List<Projectile> projectiles)
        {
            for (int i = projectiles.Count-1; i >= 0; i--)
            {
                var playerBounds = new Rectangle((int)Player.Location.X, (int)Player.Location.Y, Player.spriteWidth, Player.spriteHeight);
                var projectileBounds = new Rectangle((int)projectiles[i].Location.X, (int)projectiles[i].Location.Y,
                    projectiles[i].side, projectiles[i].side);

                if (projectileBounds.Intersects(playerBounds))
                {
                    Connection.SendRemove(projectiles[i]);
                    projectiles.Remove(projectiles[i]);
                    Player.Health--;
                    Player.DamageEffect = 5;

                    if (GameMain.Settings.SoundEnabled)
                    {
                        hitSound.Play();
                    }

                    if (Player.Health <= 0)
                    {
                        Connection.SendVictory();
                        GameEnd(false);
                    }
                }
            }
        }

        public void GameEnd(bool victory)
        {
            string message = victory ? "¡Has derrotado al jugador contrario y has ganado la partida!" :
                "Tu salud se ha quedado a 0 y te han vencido...";

            Connection.Disconnect();

            //mostrar mensaje
        }
    }
}
