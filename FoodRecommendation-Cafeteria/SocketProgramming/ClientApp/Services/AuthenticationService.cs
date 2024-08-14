using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

public class AuthenticationService
{
    private const string server = "127.0.0.1";
    private const int port = 8080;

    public static int UserId { get; private set; }

    public AuthenticationResult Authenticate(string name, string password)
    {
        try
        {
            TcpClient client = new TcpClient(server, port);
            using (NetworkStream stream = client.GetStream())
            {
                CustomData authData = new CustomData
                {
                    UserData = new UserData
                    {
                        Name = name,
                        Password = password,
                    },
                    Choice = "authenticate"
                };

                string authJsonData = JsonConvert.SerializeObject(authData);
                byte[] authDataBytes = Encoding.ASCII.GetBytes(authJsonData);

                stream.Write(authDataBytes, 0, authDataBytes.Length);

                byte[] authResponseBuffer = new byte[1024];
                int authBytesRead = stream.Read(authResponseBuffer, 0, authResponseBuffer.Length);
                string authResponse = Encoding.ASCII.GetString(authResponseBuffer, 0, authBytesRead);

                AuthenticationResult responseData = JsonConvert.DeserializeObject<AuthenticationResult>(authResponse);
                UserId = responseData.UserID;
                new UserData { UserID = responseData.UserID};

                return new AuthenticationResult { Authenticated = true, UserRole = responseData.UserRole };
            }
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"SocketException: {ex.Message}");
            return new AuthenticationResult { Authenticated = false };
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JsonException during authentication: {ex.Message}");
            return new AuthenticationResult { Authenticated = false };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during authentication: {ex.Message}");
            return new AuthenticationResult { Authenticated = false };
        }
    }
}
