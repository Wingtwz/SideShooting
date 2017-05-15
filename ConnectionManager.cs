using System.IO;
using System.Net;
using System.Net.Sockets;
using Microsoft.Xna.Framework;

namespace ProyectoDAM
{
    public class ConnectionManager
    {
        public Socket Socket { get; set; }

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
            }
        }

        public void SendPosition(Vector2 location)
        {
            sw.WriteLine($"POSITION {location.X} {location.Y}");
            sw.Flush();
        }
    }
}
