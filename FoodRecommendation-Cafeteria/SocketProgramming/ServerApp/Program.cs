using System.Net;
using System.Net.Sockets;

class Program
{
    private const int port = 8080;
    private static string connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

    static void Main(string[] args)
    {
        TcpListener server = new TcpListener(IPAddress.Any, port);
        server.Start();
        Console.WriteLine("Server started...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Client connected...");

            var clientThread = new Thread(new ParameterizedThreadStart(ClientHandler.HandleClient));
            clientThread.Start(client);
        }
    }
}