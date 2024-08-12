using MySqlConnector;

public static class Logout
{
    private static string connectionString = Configuration.ConnectionString;

    public static void LogUserLogout(int userId)
    {
        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO UserSessionLog (UserId, LogoutTime) VALUES (@userId, @LogoutTime)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@LogoutTime", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"MySQL error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General error: {ex.Message}");
        }
    }
}
