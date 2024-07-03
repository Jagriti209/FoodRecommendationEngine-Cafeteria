﻿using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

public class Client
{
    public const string Server = "127.0.0.1";
    public const int Port = 8080;

    public void SendDataToServer(CustomData data)
    {
        try
        {
            string jsonData = JsonConvert.SerializeObject(data);
            SendRequestAndGetResponse(jsonData);
            Console.WriteLine("Data sent to server");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }

    public string SendRequestAndGetResponse(string requestData)
    {
        try
        {
            TcpClient client = ConnectToServer();
            if (client == null)
                return null;

            using (NetworkStream stream = client.GetStream())
            {
                byte[] dataBytes = Encoding.ASCII.GetBytes(requestData);
                stream.Write(dataBytes, 0, dataBytes.Length);

                        byte[] responseBuffer = new byte[1024];
                int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
                string response = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead);

                return response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during request/response: {ex.Message}");
            return null;
        }
    }

    private TcpClient ConnectToServer()
    {
        try
        {
            TcpClient client = new TcpClient(Server, Port);
            return client;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception while connecting to server: {ex.Message}");
            return null;
        }
    }
}