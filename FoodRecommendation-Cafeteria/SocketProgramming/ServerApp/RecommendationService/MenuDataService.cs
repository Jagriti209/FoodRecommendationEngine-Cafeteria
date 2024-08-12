using MySqlConnector;

public class MenuDataService
{
    private readonly string connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

    public List<MenuItem> GetMenuItemsByMealType(string mealType)
    {
        List<MenuItem> menuItems = new List<MenuItem>();
        using (var connection = new MySqlConnection(connectionString))
            try
            {
                connection.Open();
                string query = "SELECT menuID, itemName FROM Menu WHERE mealType = @mealType and availability = @availability";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@mealType", mealType);
                    command.Parameters.AddWithValue("@availability", 1);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            menuItems.Add(new MenuItem
                            {
                                MenuID = reader.GetInt32("menuID"),
                                ItemName = reader.GetString("itemName"),
                                MealType = mealType
                            });
                        }
                    }
                }
            }
            catch (MySqlException sqlEx)
            {
                Console.WriteLine($"MySQL error while retrieving menu items: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error while retrieving menu items: {ex.Message}");
            }

        return menuItems;
    }
}
