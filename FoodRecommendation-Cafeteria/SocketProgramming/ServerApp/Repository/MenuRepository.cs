using MySqlConnector;

public class MenuRepository
{
    private readonly string connectionString;

    public MenuRepository()
    {
        this.connectionString = Configuration.ConnectionString;
    }

    public List<MenuItem> FetchMenuItems()
    {
        var menuItems = new List<MenuItem>();

        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT menuID, itemName, price, availability, mealType FROM Menu where availability = 1";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var menuItem = new MenuItem
                            {
                                MenuID = reader.GetInt32("menuID"),
                                ItemName = reader.GetString("itemName"),
                                Price = reader.GetDecimal("price"),
                                Availability = reader.GetBoolean("availability"),
                                MealType = reader.GetString("mealType")
                            };
                            menuItems.Add(menuItem);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching menu items: {ex.Message}");
        }

        return menuItems;
    }
}
