using System;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

public static class ClientHandler
{
    private static string? roleType;
    public static void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;
        

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
                        roleType = AuthenticationManager.AuthenticateUser(requestData.Name, requestData.Password);
                    }

                    if (roleType != null)
                    {
                        requestData.RoleType = roleType;
                        string responseDataJson = JsonConvert.SerializeObject(requestData);
                        byte[] responseDataBytes = Encoding.ASCII.GetBytes(responseDataJson);
                        
                        if (requestData.Choice == "authenticate")
                        {
                            stream.Write(responseDataBytes, 0, responseDataBytes.Length);
                        }

                        if (requestData.Choice != "authenticate")
                        {
                            RoleBasedAction(stream, roleType, requestData);
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

    private static void RoleBasedAction(NetworkStream stream, string roleType, CustomData clientChoiceData)
    {
        Console.WriteLine(roleType,"as");
        switch (roleType.ToLower())
        {
            case "admin":
                OperationsManager.ProcessAdminAction(stream, clientChoiceData);
                break;
/*            case "chef":
                OperationsManager.ProcessChefAction(stream, clientChoiceData);
                break;*/
            case "employee":
                OperationsManager.ProcessEmployeeAction(stream, clientChoiceData);
                break;
            default:
                Console.WriteLine("Invalid role type.");
                break;
        }
    }
}


