using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Xna.Framework;
using SideShooting.Elements;
using SideShooting.Screens;

namespace SideShooting.Handlers
{
    /// <summary>
    /// Gestiona el intercambio de datos con el servidor
    /// </summary>
    public class ConnectionManager
    {
        /// <summary>
        /// <see cref="Socket"/> de la conexión con el servidor para el tráfico de información
        /// </summary>
        public Socket Socket { get; set; }
        /// <summary>
        /// Personaje remoto según los datos recibidos por el servidor
        /// </summary>
        public Character Character { get; set; }
        /// <summary>
        /// Indica si la conexión con el servidor está activa
        /// </summary>
        public bool ConnectionAlive { get; set; }
        /// <summary>
        /// <see cref="GameScreen"/> actualmente en uso en el juego
        /// </summary>
        public GameScreen GameScreen { get; set; }

        /// <summary>
        /// <see cref="NetworkStream"/> en uso
        /// </summary>
        private NetworkStream ns;
        /// <summary>
        /// <see cref="StreamReader"/> en uso
        /// </summary>
        private StreamReader sr;
        /// <summary>
        /// <see cref="StreamWriter"/> en uso
        /// </summary>
        private StreamWriter sw;

        /// <summary>
        /// Realiza la conexión con el servidor y guarda el <see cref="Socket"/> resultante
        /// </summary>
        /// <param name="IP">IP del servidor</param>
        /// <param name="port">Puerto del servidor</param>
        /// <returns>True en caso de conectarse y estar listo para jugar, false en caso de conectarse
        /// pero falten jugadores, y null en caso de que haya algún problema al conectarse</returns>
        public bool? Connect(string IP, int port)
        {
            bool? ready = false;
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), port);
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket.Connect(ep);
                Socket.SendTimeout = 5;

                ns = new NetworkStream(Socket);
                sr = new StreamReader(ns);
                sw = new StreamWriter(ns);

                if (sr.ReadLine() != "ConectadoASideShooting")
                {
                    throw new SocketException();
                }
                else
                {
                    sw.WriteLine("OKSS");
                    sw.Flush();

                    if (sr.ReadLine() == "READY")
                    {
                        ready = true;
                    }

                    ConnectionAlive = true;

                    Thread thread = new Thread(Receiver);
                    thread.Start();
                }
            }
            catch (FormatException)
            {
                ready = null;
            }

            return ready;
        }

        /// <summary>
        /// Prepara una instancia de la clase <see cref="GameScreen"/> y se ajusta como escena en uso
        /// </summary>
        /// <remarks>Se llama cuando ya se dispone de dos jugadores para poner al jugador en espera
        /// en la escena de juego correcta</remarks>
        public void ContinueToGame()
        {
            var game = new GameScreen(GameMain.currentScreen.Content, GameMain.currentScreen.GraphicsDevice, this);
            game.Player.Location = new Vector2(100, 350);
            GameMain.currentScreen = game;
        }

        /// <summary>
        /// Realiza las desconexiones del servidor
        /// </summary>
        public void Disconnect()
        {
            sw.Close();
            sr.Close();
            ns.Close();
            Socket.Close();
            ConnectionAlive = false;
        }

        /// <summary>
        /// Se encarga de recibir los comandos del servidor y gestionar que debe hacer según
        /// la información que reciba en cada mensaje.
        /// </summary>
        public void Receiver()
        {
            string message;
            string[] data;

            try
            {
                while (ConnectionAlive)
                {
                    message = sr.ReadLine();

                    if (message == null)
                    {
                        GameScreen.GameEnd(true);
                        break;
                    }

                    if (message == "GO")
                        ContinueToGame();

                    data = message.Split(' ');

                    if (Character != null)
                    {
                        switch (data[0])
                        {
                            case "GO":
                                ContinueToGame();
                                break;

                            case "LOCATION":
                                this.Character.Location = new Vector2(float.Parse(data[1]), float.Parse(data[2]));
                                this.Character.CurrentAnimation = int.Parse(data[3]);
                                this.Character.CurrentFrame = int.Parse(data[4]);
                                break;

                            case "PROJECTILE":
                                var p = new Projectile(GameScreen.ProjectileSprite,
                                    new Vector2(float.Parse(data[1]), float.Parse(data[2])),
                                    new Vector2(float.Parse(data[3]), float.Parse(data[4])), Convert.ToInt32(data[5]));
                                lock(GameScreen.l)
                                    this.GameScreen.EnemyProjectiles.Add(p);
                                break;

                            case "REMOVE":
                                lock (GameScreen.l)
                                {
                                    for (int i = GameScreen.Projectiles.Count - 1; i >= 0; i--)
                                    {
                                        if (GameScreen.Projectiles[i].Id == Convert.ToInt32(data[1]))
                                        {
                                            GameScreen.Projectiles.Remove(GameScreen.Projectiles[i]);
                                            GameScreen.Player.ProjectilesAvailable++;
                                        }
                                    }
                                }
                                break;

                            case "VICTORY":
                                GameScreen.GameEnd(true);
                                break;

                            case "CLEANER":
                                GameScreen.DoCleaner();
                                break;
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Envía la posición actual de un jugador al servidor
        /// </summary>
        /// <param name="location">Posición del jugador</param>
        /// <param name="currentAnimation">Animación que está haciendo el jugador <see cref="Character.CurrentAnimation"/></param>
        /// <param name="currentFrame">Frame actual que se debe dibujar de la animación</param>
        public void SendPosition(Vector2 location, int currentAnimation, int currentFrame)
        {
            sw.WriteLine($"LOCATION {location.X} {location.Y} {currentAnimation} {currentFrame}");
            sw.Flush();
        }

        /// <summary>
        /// Envía los datos necesarios al servidor para generar proyectiles remotos
        /// </summary>
        /// <param name="p">Proyectil a enviar</param>
        public void SendProjectile(Projectile p)
        {
            sw.WriteLine($"PROJECTILE {p.Location.X} {p.Location.Y} {p.Acceleration.X} {p.Acceleration.Y} {p.Id}");
            sw.Flush();
        }

        /// <summary>
        /// Envía una orden de borrado de un proyectil concreto al servidor
        /// </summary>
        /// <param name="p">Proyectil a borrar</param>
        public void SendRemove(Projectile p)
        {
            sw.WriteLine($"REMOVE {p.Id}");
            sw.Flush();
        }

        /// <summary>
        /// Envía una indicación de victoria del otro jugador al servidor
        /// </summary>
        public void SendVictory()
        {
            sw.WriteLine("VICTORY");
            sw.Flush();
        }

        /// <summary>
        /// Envía una orden para borrar todos los proyectiles al servidor
        /// </summary>
        public void SendCleaner()
        {
            sw.WriteLine("CLEANER");
            sw.Flush();
        }
    }
}
