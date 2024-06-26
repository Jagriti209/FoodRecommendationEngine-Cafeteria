using System;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

public class AuthenticationService
{
    private const string server = "127.0.0.1";
    private const int port = 8080;

    public AuthenticationResult Authenticate(string name, string password)
    {
        try
        {
            TcpClient client = new TcpClient(server, port);
            NetworkStream stream = client.GetStream();


            CustomData authData = new CustomData
            {
                Name = name,
                Password = password,
                Choice = "authenticate"
            };
            string authJsonData = JsonConvert.SerializeObject(authData);
            byte[] authDataBytes = Encoding.ASCII.GetBytes(authJsonData);
            stream.Write(authDataBytes, 0, authDataBytes.Length);
            Console.WriteLine("Authentication data sent to server");

            byte[] authResponseBuffer = new byte[1024];
            int authBytesRead = stream.Read(authResponseBuffer, 0, authResponseBuffer.Length);
            string authResponse = Encoding.ASCII.GetString(authResponseBuffer, 0, authBytesRead);
            Console.WriteLine($"Authentication response: {authResponse}");



            CustomData responseData = JsonConvert.DeserializeObject<CustomData>(authResponse);

            string roleType = responseData.RoleType;

            stream.Close();
            client.Close();

            return new AuthenticationResult { Authenticated = true, UserRole = roleType };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during authentication: {ex.Message}");
            return new AuthenticationResult { Authenticated = false };
        }
    }
}

public class AuthenticationResult
{
    public bool Authenticated { get; set; }
    public string UserRole { get; set; }
}
