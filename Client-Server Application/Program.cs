using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client_Server_Application
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string ipAddress = "127.0.0.1";
            const int port = 8080;

            var endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);

            var currentSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                currentSocket.Bind(endPoint);
                currentSocket.Listen(5);

                while (true)
                {
                    var handler = currentSocket.Accept();
                    var buffer = new byte[256];
                    var size = 0;
                    var data = new StringBuilder();

                    do
                    {
                        size = handler.Receive(buffer);
                        data.Append(Encoding.Unicode.GetString(buffer, 0, size));
                        
                    }
                    while (handler.Available > 0);

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + data.ToString());

                    handler.Send(Encoding.Unicode.GetBytes("Your message has been delivered"));

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
