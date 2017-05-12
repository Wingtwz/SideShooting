using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ProyectoDAM
{
    class ConnectionManager
    {
        public Socket Socket { get; set; }

        public void Connect(string IP, int port)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), port);
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Connect(ep);
            Socket.SendTimeout = 5;

            NetworkStream ns = new NetworkStream(Socket);
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);

            if (sr.ReadLine() != "ConectadoASideShooting")
            {
                throw new SocketException();
            }
            else
            {
                //gestionar una vez conectado
            }
        }
    }
}
