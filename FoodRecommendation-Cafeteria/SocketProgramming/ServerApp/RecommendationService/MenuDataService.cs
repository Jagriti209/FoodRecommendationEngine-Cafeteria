using MySqlConnector;
using System.Collections.Generic;

public class MenuDataService
{
    private readonly string _connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

    public List<MenuItem> GetMenuItemsByMealType(string mealType)
    {
        List<MenuItem> menuItems = new List<MenuItem>();

        using (var connection = new MySqlConnection(_connectionString))
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

        return menuItems;
    }
}
