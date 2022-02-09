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
                currentSocket.Listen(1);

                while (true)
                {
                    var handler = currentSocket.Accept();
                    var buffer = new byte[256];
                    var size = 0;
                    var data = new CMessage();

                    do
                    {
                        size = handler.Receive(buffer);
                        var obj= CustomConverter.Deserialize(buffer);
                        if(obj is CMessage)
                            data = (CMessage)obj;
                        
                    }
                    while (handler.Available > 0);

                    //TODO : add saving messages

                    handler.Send(Encoding.Unicode.GetBytes("Your message has been delivered\n"));

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();

                    if (data.Message == "exit")
                        break;

                    Console.WriteLine(data.Id + ")" + DateTime.Now.ToShortTimeString() + ": " + data.Message);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
