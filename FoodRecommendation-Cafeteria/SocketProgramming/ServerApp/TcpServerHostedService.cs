/*using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class TcpServerHostedService : BackgroundService
{
    private const int Port = 8080;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        TcpListener server = new TcpListener(IPAddress.Any, Port);
        server.Start();
        Console.WriteLine("Server started...");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                Console.WriteLine("Client connected...");
                _ = Task.Run(() => HandleClientAsync(client, stoppingToken));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in server: {ex.Message}");
        }
        finally
        {
            server.Stop();
        }
    }

    private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
    {
        try
        {
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) != 0)
                {
                    string json = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received JSON: {json}");

                    CustomData data = JsonConvert.DeserializeObject<CustomData>(json);
                    Console.WriteLine($"Received Data - Name: {data.Name}, UserId: {data.UserId}, RoleType: {data.RoleType}");

                    await DatabaseService.SaveToDatabaseAsync(data);
                    await SendAcknowledgmentAsync(stream);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in client handler: {ex.Message}");
        }
    }

    private async Task SendAcknowledgmentAsync(NetworkStream stream)
    {
        string response = "Data received and stored in database";
        byte[] responseBytes = Encoding.ASCII.GetBytes(response);
        await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
        Console.WriteLine("Acknowledgment sent");
    }
}*/