using MySqlConnector;
using System;

public class RecommendationDataService
{
    private readonly string _connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

    public void InsertRecommendation(MenuItemScore menuItemScore, string mealType)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "INSERT INTO foodrecommendation (menuID, mealType, itemReview, recommendationDate) " +
                           "VALUES (@menuID, @mealType, @itemReview, CURDATE())";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@menuID", menuItemScore.MenuID);
                command.Parameters.AddWithValue("@mealType", mealType);
                command.Parameters.AddWithValue("@itemReview", menuItemScore.AverageScore.ToString());
                command.Parameters.AddWithValue("@recommendationDate", DateTime.Now);

                command.ExecuteNonQuery();
            }
        }
    }
}
