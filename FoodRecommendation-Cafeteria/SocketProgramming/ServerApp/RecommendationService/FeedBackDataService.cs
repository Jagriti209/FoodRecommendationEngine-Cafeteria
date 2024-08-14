using MySqlConnector;
using System.Collections.Generic;

public class FeedbackDataService
{
    private readonly string _connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

    public List<FeedbackData> GetFeedbacksByMenuId(int menuID)
    {
        List<FeedbackData> feedbacks = new List<FeedbackData>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT feedback, rating FROM Feedback WHERE menuID = @menuID and feedback is not null";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@menuID", menuID);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        feedbacks.Add(new FeedbackData
                        {
                            Feedback = reader.GetString("feedback"),
                            Rating = reader.GetInt32("rating")
                        });
                    }
                }
            }
        }

        return feedbacks;
    }
}
