using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SideShooting.Elements;
using SideShooting.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace SideShooting.Screens
{
    /// <summary>
    /// Contiene los métodos y parámetros de la escena "jugable" del juego
    /// </summary>
    public class GameScreen : Screen
    {
        /// <summary>
        /// Personaje al que controla el jugador
        /// </summary>
        public Character Player { get; set; }
        /// <summary>
        /// Textura para los proyectiles del juego
        /// </summary>
        public Texture2D ProjectileSprite { get; set; }
        /// <summary>
        /// Colección de los proyectiles disparados por el jugador
        /// </summary>
        public List<Projectile> Projectiles { get; set; }
        /// <summary>
        /// Colección de los proyectiles disparador por el rival
        /// </summary>
        public List<Projectile> EnemyProjectiles { get; set; }
        /// <summary>
        /// Instancia del manejador de conexión
        /// </summary>
        public ConnectionManager Connection { get; set; }

        /// <summary>
        /// Sonidos del juego. Hit: Golpe recibido. Shot: Disparo realizado.
        /// </summary>
        public SoundEffect hitSound, shotSound;
        /// <summary>
        /// Objeto para evitar problemas de concurrencia
        /// </summary>
        public readonly object l = new object();

        /// <summary>
        /// Textura para <see cref="Character"/>
        /// </summary>
        private Texture2D characterSprite;
        /// <summary>
        /// Textura con el mapa de fondo del juego
        /// </summary>
        private Texture2D map;
        /// <summary>
        /// Textura para la capa de nubes del juego
        /// </summary>
        private Texture2D clouds;
        /// <summary>
        /// Textura con la interfaz del juego
        /// </summary>
        private Texture2D ui;
        /// <summary>
        /// Textura sobre la que mostrar mensajes relevantes en el juego
        /// </summary>
        private Texture2D messageImage;
        /// <summary>
        /// Fuente para dibujar mensajes en el juego
        /// </summary>
        private SpriteFont font;
        /// <summary>
        /// Colección de vectores que contienen la posición que deben tener las capas de nubes
        /// </summary>
        private Vector2[] cloudVectors = new Vector2[4];
        /// <summary>
        /// Canción principal del juego
        /// </summary>
        private Song gameSong;
        /// <summary>
        /// Rectángulos con información relevante para la interfaz del juego.
        /// </summary>
        /// <remarks>Rect: Zona a dibujar de la textura de la interfaz. RedRect: Cantidad de rojo a dibujar de su barra.
        /// GreenBar: Cantidad de verde a dibujar de su barra. BlueBar: Cantidad de azul a dibujar de su barra.</remarks>
        private Rectangle uiRect, uiRedRect, uiGreenBar, uiBlueBar;
        /// <summary>
        /// Tiempo transcurrido desde el último envío de información al servidor
        /// </summary>
        /// <remarks>Dado que no es necesario enviar información constantemente al servidor,
        /// se envía la información de posición del personaje 20 veces por segundo frente
        /// a las 60 que lo haría por defecto.</remarks>
        private float timeSinceLastTick;
        /// <summary>
        /// Indica cuantos frames de animación de borrado (pantallazo azul) han de dibujarse (la mitad de este valor)
        /// </summary>
        private int cleanerEffect;
        /// <summary>
        /// Mensaje a mostrar al final de la partida con la información de victoria/derrota
        /// </summary>
        private string endResult;
        /// <summary>
        /// Indica si la partida ha finalizado
        /// </summary>
        private bool gameEnded;

        /// <summary>
        /// Crea una nueva instancia de la escena de juego
        /// </summary>
        /// <param name="content">Gestor de contenido en uso</param>
        /// <param name="graphicsDevice">Interfaz sobre la que dibujar en uso</param>
        /// <param name="connection">Instancia de <see cref="ConnectionManager"/> con la que se está conectado al servidor</param>
        public GameScreen(ContentManager content, GraphicsDevice graphics, ConnectionManager connection) : base (content, graphics)
        {
            this.LoadContent();
            this.Player = new Character(characterSprite);
            this.Connection = connection;
            this.Connection.Character = new Character(characterSprite);
            this.Connection.GameScreen = this;
            this.Projectiles = new List<Projectile>();
            this.EnemyProjectiles = new List<Projectile>();
            this.uiRect = new Rectangle(8, 8, 100, 32);
            cleanerEffect = 0;
            gameEnded = false;
            timeSinceLastTick = 0;

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

        /// <summary>
        /// Desde aquí se dibuja el mapa, la capa de nubes y se llama al método del personaje correspondiente
        /// para que se dibuje adecuadamente, además de otros efectos/animaciones.
        /// </summary>
        /// <param name="gameTime">Valor de tiempo actual</param>
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

            int basePos = 15;
            SpriteBatch.Draw(ui, new Vector2(basePos), uiRect, Color.White);
            SpriteBatch.Draw(ui, new Vector2(basePos + 36, basePos + 3), uiRedRect, Color.White);
            SpriteBatch.Draw(ui, new Vector2(basePos + 36, basePos + 13), uiGreenBar, Color.White);
            SpriteBatch.Draw(ui, new Vector2(basePos + 36, basePos + 23), uiBlueBar, Color.White);

            SpriteBatch.DrawString(font, ""+Player.ProjectilesAvailable, new Vector2(basePos + 12, basePos + 3), Color.White);

            if (gameEnded)
            {
                int x = 380, y = 400;
                SpriteBatch.Draw(messageImage, new Rectangle(x, y, messageImage.Width * 2, messageImage.Height * 2), Color.White);
                SpriteBatch.DrawString(font, endResult + "\nHaz click para volver al menu", new Vector2(x + 40, y + 20), Color.White);
            }

            SpriteBatch.End();

            if (Player.DamageEffect % 2 != 0)
            {
                GraphicsDevice.Clear(Color.DarkRed);
            }

            if (cleanerEffect % 2 != 0)
            {
                GraphicsDevice.Clear(Color.Blue);
            }
        }

        /// <summary>
        /// Desde aquí se actualiza toda la lógica del juego, posiciones de proyectiles o gestión de pulsaciones.
        /// </summary>
        /// <param name="gameTime">Valor de tiempo actual</param>
        /// <param name="gameActive">Indica si la pantalla de juego tiene el foco del sistema</param>
        public override void Update(GameTime gameTime, bool gameActive)
        {
            try
            {
                if (!gameEnded)
                {
                    if (gameActive)
                    {
                        InputManager.Game(this, gameTime);
                    }

                    this.UpdateProjectiles(gameTime);

                    this.CheckCollisions(EnemyProjectiles);

                    for (int i = 0; i < cloudVectors.Length; i++)
                    {
                        cloudVectors[i] = new Vector2(cloudVectors[i].X < -1279 ? 1280 : cloudVectors[i].X - 1, cloudVectors[i].Y > 720 ? -720 : cloudVectors[i].Y + 1);
                    }

                    if (Player.DamageEffect > 0)
                    {
                        Player.DamageEffect--;
                    }

                    if (cleanerEffect > 0)
                    {
                        cleanerEffect--;
                    }

                    uiRedRect = new Rectangle(120, 11, (this.Player.Health * 52) / Character.MaxHealth, 6);
                    uiGreenBar = new Rectangle(120, 21, (this.Player.GreenBar * 52) / Character.MaxGreen, 6);
                    uiBlueBar = new Rectangle(120, 31, (this.Player.BlueBar * 52) / Character.MaxBlue, 6);

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
                    if (gameActive && InputManager.GameEnded())
                    {
                        GameMain.currentScreen = new MenuScreen(Content, GraphicsDevice);
                    }
                }
            }
            catch (ObjectDisposedException) { }
            catch (IOException e)
            {
                Debug.WriteLine(e.Message);
                var menu = new MenuScreen(Content, GraphicsDevice);
                menu.TextStatus = "Error de conexion con el servidor";
                GameMain.currentScreen = menu;
            }
        }

        /// <summary>
        /// Desde aquí se realiza la carga de contenido de archivos
        /// </summary>
        public override void LoadContent()
        {
            characterSprite = Content.Load<Texture2D>("Images/character");
            ProjectileSprite = Content.Load<Texture2D>("Images/proyectil");
            map = Content.Load<Texture2D>("Images/mapahierba");
            gameSong = Content.Load<Song>("Audio/juego2");
            hitSound = Content.Load<SoundEffect>("Audio/enemigotocado");
            shotSound = Content.Load<SoundEffect>("Audio/dmgrecibido");
            clouds = Content.Load<Texture2D>("Images/capanubes");
            ui = Content.Load<Texture2D>("Images/ui");
            font = Content.Load<SpriteFont>("Fonts/SaviorMessage");
            messageImage = Content.Load<Texture2D>("Images/message");
        }

        /// <summary>
        /// Para dibujar los proyectiles que contiene una colección
        /// </summary>
        /// <param name="projectiles">Colección de proyectiles a dibujar</param>
        public void DrawProjectiles(List<Projectile> projectiles)
        {
            lock (l)
            {
                foreach (var p in projectiles)
                {
                    p.Draw(SpriteBatch);
                }
            }
        }

        /// <summary>
        /// Actualiza la posición de las colecciones de proyectiles
        /// </summary>
        /// <param name="gameTime">Valor de tiempo actual</param>
        public void UpdateProjectiles(GameTime gameTime)
        {
            lock (l)
            {
                for (int i = Projectiles.Count - 1; i >= 0; i--)
                {
                    Projectiles[i].Update(gameTime);

                    if (!GameMain.ScreenRect.Contains(Projectiles[i].Location.ToPoint()))
                    {
                        Projectiles.Remove(Projectiles[i]);
                        Player.ProjectilesAvailable++;
                    }
                }

                for (int i = EnemyProjectiles.Count - 1; i >= 0; i--)
                {
                    EnemyProjectiles[i].Update(gameTime);

                    if (!GameMain.ScreenRect.Contains(EnemyProjectiles[i].Location.ToPoint()))
                    {
                        Projectiles.Remove(EnemyProjectiles[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Comprueba si hay alguna colisión entre el jugador y los proyectiles enemigos
        /// </summary>
        /// <param name="projectiles">Colección con los proyectiles a comprobar</param>
        public void CheckCollisions(List<Projectile> projectiles)
        {
            lock (l)
            {
                for (int i = projectiles.Count - 1; i >= 0; i--)
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
        }

        /// <summary>
        /// Parepara el fin de la partida
        /// </summary>
        /// <param name="victory">Indica si el jugador ha ganado la partida</param>
        public void GameEnd(bool victory)
        {
            Connection.Disconnect();

            endResult = victory ? "Rival derrotado, has ganado" :
                "Tu salud se ha quedado a 0 y te han vencido...";

            gameEnded = true;
        }

        /// <summary>
        /// Realiza la acción "fogueo" para borrar los proyectiles de la pantalla
        /// </summary>
        public void DoCleaner()
        {
            lock (l)
            {
                Projectiles.Clear();
                EnemyProjectiles.Clear();
            }
            cleanerEffect = 5;
            Player.ProjectilesAvailable = Character.MaxProjectiles;
        }
    }
}
