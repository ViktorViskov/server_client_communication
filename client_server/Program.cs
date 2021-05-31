using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace client_server
{
    class Program
    {
        static void Main(string[] args)
        {
            // app variables
            int port;
            IPAddress ip;
            IPEndPoint endPoint;
            TcpClient client;
            NetworkStream stream;
            TcpListener listener;
            byte[] buffer;
            byte[] answer_buffer;
            int numberOfBytesRead;
            string message;
            string answer;

            // print message
            Console.Clear();
            Console.WriteLine("App for demonstration server - client communication");
            Console.WriteLine("Press key 1 to open client side");
            Console.WriteLine("Press key 2 to open server side");


            // user action
            switch (Console.ReadKey(true).Key)
            {
                // 
                // client side
                // 

                case ConsoleKey.D1:
                    // user input
                    Console.WriteLine("Input data for connection");
                    Console.Write("IP address: ");
                    string ip_addr = Console.ReadLine();
                    Console.Write("Port: ");
                    port = int.Parse(Console.ReadLine());
                    Console.Write("Message: ");
                    message = Console.ReadLine();

                    // variables
                    ip = IPAddress.Parse(ip_addr);
                    endPoint = new IPEndPoint(ip, port);

                    // message
                    buffer = Encoding.UTF8.GetBytes(message);
                    answer_buffer = new byte[256];

                    // open client socket
                    client = new TcpClient();

                    // open conntecion
                    client.Connect(endPoint);

                    // data stream
                    stream = client.GetStream();
                    stream.Write(buffer, 0, buffer.Length);

                    // get data from server
                    numberOfBytesRead = stream.Read(answer_buffer, 0, 256);
                    answer = Encoding.UTF8.GetString(answer_buffer, 0, numberOfBytesRead);
                    Console.WriteLine(answer);

                    // clouse stream
                    client.Close();
                    break;

                // 
                // server side
                // 
                case ConsoleKey.D2:

                    // variables
                    port = 13356;
                    ip = IPAddress.Any;
                    endPoint = new IPEndPoint(ip, port);
                    buffer = new byte[256];

                    // server listener
                    listener = new TcpListener(endPoint);
                    listener.Start();

                    // print status message
                    Console.WriteLine($"Local machine: {Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString()}");
                    Console.WriteLine($"Server ip address: {listener.LocalEndpoint.ToString()}");
                    Console.WriteLine($"Server port: {port}");
                    Console.WriteLine("Awaiting clients");

                    // awaiting for connections
                    client = listener.AcceptTcpClient();

                    // creating stream betwen client server
                    stream = client.GetStream();

                    // load data from client
                    numberOfBytesRead = stream.Read(buffer, 0, 256);

                    // convert to string message
                    message = Encoding.UTF8.GetString(buffer, 0, numberOfBytesRead);

                    // print message
                    Console.WriteLine($"{client.Client.RemoteEndPoint.ToString().Split(':')[0]} send: {message}");

                    // send answer
                    answer = $"Message '{message}' was received";
                    buffer = Encoding.UTF8.GetBytes(answer);
                    stream.Write(buffer, 0, buffer.Length);

                    // close conection
                    client.Close();
                    break;
            }

        }
    }
}
