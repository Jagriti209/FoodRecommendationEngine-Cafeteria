using MySqlConnector;

public static class AdminRequestHandler
{
    private static string connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";
    public static void AddMenuItem(MenuItem menuItem)

    {

        using (var connection = new MySqlConnection(connectionString))

        {

            try

            {

                connection.Open();

                string query = "INSERT INTO Menu (itemName, price, availability) VALUES (@name, @price, @available)";

                using (var command = new MySqlCommand(query, connection))

                {

                    command.Parameters.AddWithValue("@name", menuItem.Name);

                    command.Parameters.AddWithValue("@price", menuItem.Price);

                    command.Parameters.AddWithValue("@available", menuItem.Availability);

                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine($"{rowsAffected} row(s) inserted.");

                }

            }

            catch (Exception ex)

            {

                Console.WriteLine($"Error adding menu item: {ex.Message}");

            }

        }

    }



    public static void UpdateMenuItem(MenuItem menuItem)
    {
        Console.WriteLine("update");
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = "UPDATE Menu SET price = @price, availability = @availability WHERE itemName = @itemName";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@price", menuItem.Price);
                    command.Parameters.AddWithValue("@availability", menuItem.Availability);
                    command.Parameters.AddWithValue("@itemName", menuItem.Name);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) updated.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating menu item: {ex.Message}");
            }
        }
    }




    public static void DeleteMenuItem(string itemName)

    {

        using (var connection = new MySqlConnection(connectionString))

        {

            try

            {

                connection.Open();

                string query = "DELETE FROM Menu WHERE itemName = @itemName";

                using (var command = new MySqlCommand(query, connection))

                {

                    command.Parameters.AddWithValue("@itemName", itemName);

                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine($"{rowsAffected} row(s) deleted.");

                }

            }

            catch (Exception ex)

            {

                Console.WriteLine($"Error deleting menu item: {ex.Message}");

            }

        }

    }
}
