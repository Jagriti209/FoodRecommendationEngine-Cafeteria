using System;
using System.Net;
using System.Net.Sockets;

class Program
{
    private const int Port = 8080;
    private static readonly string ConnectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

    static void Main(string[] args)
    {
        TcpListener server = null;

        try
        {
            server = new TcpListener(IPAddress.Any, Port);
            server.Start();
            Console.WriteLine("Server started...");

            while (true)
            {
                TcpClient client = null;

                try
                {
                    client = server.AcceptTcpClient();
                    Console.WriteLine("Client connected...");

                    var clientThread = new Thread(new ParameterizedThreadStart(ClientHandler.HandleClient));
                    clientThread.Start(client);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"SocketException: {ex.Message}");
                    client?.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    client?.Close(); 
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Server exception: {ex.Message}");
        }
        finally
        {
            server?.Stop();
        }
    }
}
