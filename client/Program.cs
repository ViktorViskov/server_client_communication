// libs
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace client
{
    // class for application
    class Program
    {

        // application
        static void Main(string[] args)
        {
            // user input
            Console.WriteLine("Input data for connection");
            Console.Write("IP address: ");
            string ip_addr = Console.ReadLine();
            Console.Write("Port: ");
            int port = int.Parse(Console.ReadLine());
            Console.Write("Message: ");
            string message = Console.ReadLine();

            // variables
            IPAddress ip = IPAddress.Parse(ip_addr);
            IPEndPoint endPoint = new IPEndPoint(ip, port);

            // message
            byte[] buffer = new byte[256];
            buffer = Encoding.UTF8.GetBytes(message);
            byte[] answer_buffer = new byte[256];

            // open client socket
            TcpClient client = new TcpClient();

            // open conntecion
            client.Connect(endPoint);

            // data stream
            NetworkStream stream = client.GetStream();
            stream.Write(buffer, 0, buffer.Length);

            // get data from server
            int arrSize = stream.Read(answer_buffer, 0, 256);
            string answer = Encoding.UTF8.GetString(answer_buffer,0, arrSize);
            Console.WriteLine(answer);

            // clouse stream
            client.Close();
        }
    }
}
