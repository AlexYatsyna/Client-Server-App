using Client_Server_Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string ipAddress = "127.0.0.1";
            const int port = 8080;
            int messageId = 0;
            try
            {
                while (true)
                {
                    var endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
                    var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    Console.WriteLine("Enter your message:");
                    messageId++;
                    var message = new CMessage() { Message = Console.ReadLine(), Id = messageId };

                    var data = CustomConverter.Serialize(message);

                    socket.Connect(endPoint);
                    socket.Send(data);

                    var buffer = new byte[256];
                    var size = 0;
                    var answer = new StringBuilder();

                    do
                    {
                        size = socket.Receive(buffer);
                        answer.Append(Encoding.Unicode.GetString(buffer, 0, size));

                    }
                    while (socket.Available > 0);

                    Console.WriteLine(answer.ToString());

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();

                    if (message.Message == "exit")
                        break;
                }
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}
