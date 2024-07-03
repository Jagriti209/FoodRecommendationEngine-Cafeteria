using MySqlConnector;
using System;

public static class ChefMenuRepository
{
    private static string _connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

    public static void CreateMenuForNextDay(MenuItem menuData)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                connection.Open();

                string query = "INSERT INTO NextDayMenu (mealType, menuID, MenuCreatedDate) VALUES (@mealType, @menuID, CURDATE())";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@mealType", menuData.MealType);
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
}
