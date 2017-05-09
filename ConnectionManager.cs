using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ProyectoDAM
{
    class ConnectionManager
    {
        public void Connect(string IP, int port)
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), port);
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s.Connect(ep);
                //s.SendTimeout(5);

                NetworkStream ns = new NetworkStream(s);
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
            catch (SocketException e)
            {
            }
        }
    }
}
