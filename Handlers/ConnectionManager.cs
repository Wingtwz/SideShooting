using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Xna.Framework;
using ProyectoDAM.Screens;
using SideShooting.Elements;

namespace ProyectoDAM
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

        public void Connect(string IP, int port)
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

                Thread thread = new Thread(Receiver);
                thread.Start();
            }
        }

        public void SendPosition(Vector2 location, int currentAnimation, int currentFrame)
        {
            sw.WriteLine($"LOCATION {location.X} {location.Y} {currentAnimation} {currentFrame}");
            sw.Flush();
        }

        public void Receiver()
        {
            string message;
            string[] data;

            while (ConnectionAlive)
            {
                message = sr.ReadLine();
                data = message.Split(' ');

                if (Character != null)
                {
                    switch (data[0])
                    {
                        case "LOCATION":
                            this.Character.Location = new Vector2(float.Parse(data[1]), float.Parse(data[2]));
                            this.Character.CurrentAnimation = int.Parse(data[3]);
                            this.Character.CurrentFrame = int.Parse(data[4]);
                            break;

                        case "PROJECTILE":
                            var p = new Projectile(GameScreen.ProjectileSprite,
                                new Vector2(float.Parse(data[1]), float.Parse(data[2])),
                                new Vector2(float.Parse(data[3]), float.Parse(data[4])));
                            this.GameScreen.EnemyProjectiles.Add(p);
                            break;
                    }
                }
            }
        }

        public void SendProjectile(Projectile p)
        {
            sw.WriteLine($"PROJECTILE {p.Location.X} {p.Location.Y} {p.Acceleration.X} {p.Acceleration.Y}");
            sw.Flush();
        }
    }
}
