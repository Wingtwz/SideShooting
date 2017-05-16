using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Xna.Framework;

namespace ProyectoDAM
{
    public class ConnectionManager
    {
        public Socket Socket { get; set; }
        public Character Character { get; set; }
        public bool ConnectionAlive { get; set; } = true;

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

                if (Character != null && data[0] == "LOCATION")
                {
                    this.Character.Location = new Vector2(float.Parse(data[1]), float.Parse(data[2]));
                    this.Character.CurrentAnimation = int.Parse(data[3]);
                    this.Character.CurrentFrame = int.Parse(data[4]);
                }
            }
        }
    }
}
