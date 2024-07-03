using MySqlConnector;

public static class AuthenticationManager
{
    private static string connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";
    public static void LogUserLogout()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO UserSessionLog (UserId, LogoutTime) VALUES (@userId, @LogoutTime)";
            using (var command = new MySqlCommand(query, connection))
            {
                //command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@LogoutTime", DateTime.Now);
                command.ExecuteNonQuery();
            }
        }
    }

    public static object AuthenticateUser(string username, string password)
    {
        AuthenticationResult authResult = new AuthenticationResult();
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = "SELECT RoleType,userID FROM User WHERE Name = @username AND Password = @password";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            authResult.UserRole = reader.GetString("RoleType");
                            authResult.UserId = reader.GetInt32("UserId");
                        }
                    }
                    LogUserLogin(authResult.UserId);
                    return authResult;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Exception during authentication: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during authentication: {ex.Message}");
                return null;
            }
        }
    }

    public static void LogUserLogin(int userId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO UserSessionLog (UserId, LoginTime) VALUES (@userId, @loginTime)";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@loginTime", DateTime.Now);
                command.ExecuteNonQuery();
            }
        }
    }
}
