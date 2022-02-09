﻿using System;
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
            const int port = 8081;
            const int remotePort = 8082;

            var endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);

            //var currentTCPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var currentUDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            

            try
            {
                #region TCPServer
                //currentSocket.Bind(endPoint);
                //currentSocket.Listen(1);

                //while (true)
                //{
                //    var handler = currentSocket.Accept();
                //    var buffer = new byte[256];
                //    var size = 0;
                //    var data = new CMessage();

                //    do
                //    {
                //        size = handler.Receive(buffer);
                //        var obj = CustomConverter.Deserialize(buffer);
                //        if (obj is CMessage)
                //            data = (CMessage)obj;

                //    }
                //    while (handler.Available > 0);

                //    //TODO : add saving messages

                //    handler.Send(Encoding.Unicode.GetBytes("Your message has been delivered\n"));

                //    handler.Shutdown(SocketShutdown.Both);
                //    handler.Close();

                //    if (data.Message == "exit")
                //        break;

                //    Console.WriteLine(data.Id + ")" + DateTime.Now.ToShortTimeString() + ": " + data.Message);
                //}
                #endregion

                currentUDPSocket.Bind(endPoint);

                while(true)
                {
                    var buffer = new byte[500];
                    var size = 0;
                    var data = new CMessage();
                    EndPoint senderEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), remotePort);

                    do
                    {
                        size = currentUDPSocket.ReceiveFrom(buffer, ref senderEndPoint);
                        var obj = CustomConverter.Deserialize(buffer);
                        if (obj is CMessage)
                            data = (CMessage)obj;

                    }
                    while (currentUDPSocket.Available > 0);

                    Console.WriteLine(data.Id + ")" + DateTime.Now.ToShortTimeString() + ": " + data.Message);

                    currentUDPSocket.SendTo(Encoding.Unicode.GetBytes("Your message has been delivered\n"), senderEndPoint);

                    if (data.Message == "exit")
                        break;
                    
                }

                currentUDPSocket.Shutdown(SocketShutdown.Both);
                currentUDPSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }
    }
}
