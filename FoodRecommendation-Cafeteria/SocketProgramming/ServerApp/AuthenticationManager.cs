using MySqlConnector;
using System.Net.Sockets;
using System.Text;

public static class AuthenticationManager
{
    private static string connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

    /*    public static string AuthenticateUser(string username, string password)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT RoleType FROM User WHERE Name = @username AND Password = @password";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        var result = command.ExecuteScalar();
                        return result != null ? result.ToString() : null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception during authentication: {ex.Message}");
                    return null;
                }
            }
        }*/

    public static void Logout(NetworkStream stream)
    {
        string responseData = "Logging out...";
        byte[] responseDataBytes = Encoding.UTF8.GetBytes(responseData);
        stream.Write(responseDataBytes, 0, responseDataBytes.Length);
        stream.Flush();
    }

    public static string AuthenticateUser(string username, string password)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = "SELECT RoleType FROM User WHERE Name = @username AND Password = @password";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    var result = command.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during authentication: {ex.Message}");
                return null;
            }
        }
    }
}
