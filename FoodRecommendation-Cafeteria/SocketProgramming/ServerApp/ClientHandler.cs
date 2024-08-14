using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

public static class ClientHandler
{
    private static string? UserRole;
    static NetworkStream stream;

    public static void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;

        AuthenticationResult authResult = new AuthenticationResult();

        try
        {
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string jsonData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                CustomData requestData = JsonConvert.DeserializeObject<CustomData>(jsonData);
                if (requestData != null)
                {

                    if (requestData.Choice == "authenticate")
                    {
                        authResult = (AuthenticationResult)AuthenticationManager.AuthenticateUser(requestData.UserData.Name, requestData.UserData.Password);
                        UserRole = authResult.UserRole;
                    }

                    if (UserRole != null)
                    {
                        string responseDataJson = JsonConvert.SerializeObject(authResult);
                        byte[] responseDataBytes = Encoding.ASCII.GetBytes(responseDataJson);

                        if (requestData.Choice == "authenticate")
                        {
                            stream.Write(responseDataBytes, 0, responseDataBytes.Length);
                        }

                        if (requestData.Choice != "authenticate")
                        {
                            RoleHandler.RoleBasedAction(stream, UserRole, requestData);
                        }

                    }
                    else
                    {
                        string errorMsg = "Authentication failed. Invalid username or password.";
                        byte[] errorBytes = Encoding.ASCII.GetBytes(errorMsg);
                        stream.Write(errorBytes, 0, errorBytes.Length);
                        Console.WriteLine("Authentication failed. Invalid credentials.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid request data received.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
        finally
        {
            if (!client.Connected)
            {
                stream.Close();
                client.Close();
                Console.WriteLine("Client disconnected...");
            }
        }
    }
    public static void SendResponse(string response)
    {
        byte[] responseDataBytes = Encoding.ASCII.GetBytes(response);
        stream.Write(responseDataBytes, 0, responseDataBytes.Length);
        stream.Flush();
    }
}


