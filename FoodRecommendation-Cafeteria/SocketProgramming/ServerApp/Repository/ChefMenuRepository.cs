using MySqlConnector;

public static class ChefMenuRepository
{
    private static string connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

    public static void CreateMenuForNextDay(MenuItem menuData)
    {
        string mealType = null;
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string fetchQuery = "SELECT mealType FROM Menu WHERE MenuID = @MenuID";
                using (var fetchCommand = new MySqlCommand(fetchQuery, connection))
                {
                    fetchCommand.Parameters.AddWithValue("@MenuID", menuData.MenuID);
                    object result = fetchCommand.ExecuteScalar();
                    if (result != null)
                    {
                        mealType = result.ToString();
                    }
                }
                string query = "INSERT INTO NextDayMenu (mealType, menuID, MenuCreatedDate) VALUES (@mealType, @menuID, CURDATE())";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@mealType", mealType);
                    command.Parameters.AddWithValue("@menuID", menuData.MenuID);
                    command.Parameters.AddWithValue("@MenuCreatedDate", DateTime.Now);
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
    }

    public static string AddItemToDiscardedMenu(int menuId)
    {
        string itemName = null;
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string fetchQuery = "SELECT itemName FROM Menu WHERE menuId = @menuId";
                using (var fetchCommand = new MySqlCommand(fetchQuery, connection))
                {
                    fetchCommand.Parameters.AddWithValue("@menuId", menuId);
                    object result = fetchCommand.ExecuteScalar();
                    if (result != null)
                    {
                        itemName = result.ToString();
                    }
                }

                string updateQuery = "UPDATE Menu SET availability = @availability WHERE MenuID = @MenuID";
                using (var command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@availability", 0);
                    command.Parameters.AddWithValue("@MenuID", menuId);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) updated.");
                }

                string insertQuery = "INSERT INTO DiscardedMenuItem (menuID, discardedDate) VALUES ( @menuID, CURDATE())";
                using (var command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@menuID", menuId);
                    command.Parameters.AddWithValue("@discardedDate", DateTime.Now);
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
        return itemName;
    }
}
