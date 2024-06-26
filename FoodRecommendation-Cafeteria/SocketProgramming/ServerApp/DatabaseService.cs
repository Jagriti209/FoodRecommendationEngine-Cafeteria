/*using MySqlConnector;

public static class DatabaseService
{
    private static string ConnectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

    public static async Task SaveToDatabaseAsync(CustomData data)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                string query = "INSERT INTO User (name, userId, password, roleType) VALUES (@name, @userId, @password, @roleType)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", data.Name);
                    command.Parameters.AddWithValue("@userId", data.UserId);
                    command.Parameters.AddWithValue("@password", data.Password);
                    command.Parameters.AddWithValue("@roleType", data.RoleType);
                    await command.ExecuteNonQueryAsync();
                    Console.WriteLine("Data saved to database");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to database: {ex.Message}");

            }
        }
    }
}
*/