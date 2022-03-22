using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DatacomLabProjectClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IPHostEntry iPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = iPHostEntry.AddressList[0];
                IPEndPoint iPEndPoint = new IPEndPoint(ipAddress, 41849);

                Socket clientSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                clientSocket.Connect(iPEndPoint);

                Console.WriteLine(">>> SUCCESSFULLY CONNECTED TO : " + clientSocket.RemoteEndPoint.ToString());

                while (true)
                {
                    byte[] messageReceived = new byte[1024];
                    int byteRecv = clientSocket.Receive(messageReceived);
                    string receivedMessage = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);

                    if (receivedMessage == "won")
                    {
                        Console.WriteLine(">>> CONGRATULATIONS!!! YOU WIN THE CONTEST.");

                        break;
                    }

                    Console.WriteLine(receivedMessage);

                    string answer = Console.ReadLine(); 

                    byte[] messageSent = Encoding.ASCII.GetBytes(answer + "<EOF>");
                    clientSocket.Send(messageSent);

                    messageReceived = new byte[1024];

                    byteRecv = clientSocket.Receive(messageReceived);
                    receivedMessage = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);

                    if (receivedMessage == "incorrect")
                    {
                        Console.WriteLine(">>> WRONG ANSWER YOU LOSE THE CONTEST, TRY AGAIN LATER.");
                        break;
                    }
                    else if (receivedMessage == "1")
                    {
                        Console.WriteLine(">>> CONGRATULATIONS!!! YOU WIN THE CONTEST.");
                        break;
                    }
                    else if (receivedMessage == "correct")
                    {
                        Console.WriteLine(">>> CONGRATULATIONS YOU GAVE THE CORRECT ANSWER, PLEASE WAIT OTHER PLAYERS TO ANSWER TO SEE THE NEXT QUESTION.");
                    }
                }

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();

                Console.WriteLine(">>> PLEASE WRITE ANYTHING TO CLOSE THE WINDOW.");

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
