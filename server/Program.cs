// libs
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace server
{
    // app class
    class Program
    {
        // application
        static void Main(string[] args)
        {
            // variables
            int port = 13356;
            IPAddress ip = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(ip, port);
            byte[] buffer = new byte[256];

            // server listener
            TcpListener listener = new TcpListener(localEndPoint);
            listener.Start();

            // print status message
            Console.WriteLine($"Local machine: {Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString()}");
            Console.WriteLine($"Server ip address: {listener.LocalEndpoint.ToString()}");
            Console.WriteLine($"Server port: {port}");
            Console.WriteLine("Awaiting clients");

            // awaiting for connections
            TcpClient client = listener.AcceptTcpClient();

            // creating stream betwen client server
            NetworkStream stream = client.GetStream();

            // load data from client
            int numberOfBytesRead = stream.Read(buffer,0,256);

            // convert to string message
            string message = Encoding.UTF8.GetString(buffer, 0, numberOfBytesRead);

            // print message
            Console.WriteLine($"{client.Client.RemoteEndPoint.ToString().Split(':')[0]} send: {message}");

            // send answer
            string answer = $"Message '{message}' was received";
            buffer = Encoding.UTF8.GetBytes(answer);
            stream.Write(buffer, 0, buffer.Length);

            // close conection
            client.Close();
        }
    }
}
