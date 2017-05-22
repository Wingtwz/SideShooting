using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Xna.Framework;
using SideShooting.Elements;
using SideShooting.Screens;

namespace SideShooting.Handlers
{
    public class ConnectionManager
    {
        public Socket Socket { get; set; }
        public Character Character { get; set; }
        public bool ConnectionAlive { get; set; } = true;
        public GameScreen GameScreen { get; set; }

        private NetworkStream ns;
        private StreamReader sr;
        private StreamWriter sw;

        public bool Connect(string IP, int port)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), port);
            bool ready = false;
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

                Thread thread = new Thread(Receiver);
                thread.Start();
            }

            return ready;
        }

        public void ContinueToGame()
        {
            var game = new GameScreen(GameMain.currentScreen.Content, GameMain.currentScreen.GraphicsDevice, this);
            game.Player.Location = new Vector2(100, 350);
            GameMain.currentScreen = game;
        }

        public void Disconnect()
        {
            sw.Close();
            sr.Close();
            ns.Close();
            Socket.Close();
            ConnectionAlive = false;
        }

        public void Receiver()
        {
            string message;
            string[] data;

            try
            {
                while (ConnectionAlive)
                {
                    message = sr.ReadLine();

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
                                this.GameScreen.EnemyProjectiles.Add(p);
                                break;

                            case "REMOVE":
                                for (int i = GameScreen.Projectiles.Count - 1; i >= 0; i--)
                                {
                                    if (GameScreen.Projectiles[i].Id == Convert.ToInt32(data[1]))
                                    {
                                        GameScreen.Projectiles.Remove(GameScreen.Projectiles[i]);
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
            catch (IOException) { }
        }

        public void SendPosition(Vector2 location, int currentAnimation, int currentFrame)
        {
            sw.WriteLine($"LOCATION {location.X} {location.Y} {currentAnimation} {currentFrame}");
            sw.Flush();
        }

        public void SendProjectile(Projectile p)
        {
            sw.WriteLine($"PROJECTILE {p.Location.X} {p.Location.Y} {p.Acceleration.X} {p.Acceleration.Y} {p.Id}");
            sw.Flush();
        }

        public void SendRemove(Projectile p)
        {
            sw.WriteLine($"REMOVE {p.Id}");
            sw.Flush();
        }

        public void SendVictory()
        {
            sw.WriteLine("VICTORY");
            sw.Flush();
        }

        public void SendCleaner()
        {
            sw.WriteLine("CLEANER");
            sw.Flush();
        }
    }
}
