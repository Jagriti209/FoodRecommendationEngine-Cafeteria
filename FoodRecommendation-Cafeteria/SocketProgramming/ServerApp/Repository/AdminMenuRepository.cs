using MySqlConnector;

public static class AdminMenuRepository
{
    private static string _connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

    public static string AddMenuItem(MenuItem menuItem)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                connection.Open();
                string query = "INSERT INTO Menu (itemName, price, availability, mealType, dateCreated, foodType, IsSpicy, cuisineType, IsSweet) VALUES (@name, @price, @available, @mealType , CURDATE(), @foodType, @IsSpicy, @cuisineType, @IsSweet)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", menuItem.ItemName);
                    command.Parameters.AddWithValue("@price", menuItem.Price);
                    command.Parameters.AddWithValue("@available", menuItem.Availability);
                    command.Parameters.AddWithValue("@mealType", menuItem.MealType);
                    command.Parameters.AddWithValue("@dateCreated", DateTime.Now);
                    command.Parameters.AddWithValue("@foodType", menuItem.FoodType);
                    command.Parameters.AddWithValue("@IsSpicy", menuItem.IsSpicy);
                    command.Parameters.AddWithValue("@cuisineType", menuItem.CuisineType);
                    command.Parameters.AddWithValue("@IsSweet", menuItem.IsSweet);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) inserted.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding menu item: {ex.Message}");
                throw;
            }
        }
        return menuItem.ItemName;
    }

    public static void UpdateMenuItem(MenuItem menuItem)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                connection.Open();
                string query = "UPDATE Menu SET price = @price, availability = @availability WHERE itemName = @itemName";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@price", menuItem.Price);
                    command.Parameters.AddWithValue("@availability", menuItem.Availability);
                    command.Parameters.AddWithValue("@itemName", menuItem.ItemName);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) updated.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating menu item: {ex.Message}");
                throw;
            }
        }
    }

    public static string DeleteMenuItem(int MenuID)
    {
        string itemName = null;
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                connection.Open();

                string fetchQuery = "SELECT itemName FROM Menu WHERE MenuID = @MenuID";
                using (var fetchCommand = new MySqlCommand(fetchQuery, connection))
                {
                    fetchCommand.Parameters.AddWithValue("@MenuID", MenuID);
                    object result = fetchCommand.ExecuteScalar();
                    if (result != null)
                    {
                        itemName = result.ToString();
                    }
                }

                string query = "DELETE FROM Menu WHERE MenuID = @MenuID";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MenuID", MenuID);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) deleted.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting menu item: {ex.Message}");
                throw;
            }
        }
        return itemName;
    }
}
