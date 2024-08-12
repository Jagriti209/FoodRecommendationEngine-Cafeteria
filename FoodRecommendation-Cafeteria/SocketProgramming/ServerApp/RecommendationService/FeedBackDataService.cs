using MySqlConnector;

public class FeedbackDataService
{
    private readonly string connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

    public List<FeedbackData> GetFeedbacksByMenuId(int menuID)
    {
        List<FeedbackData> feedbacks = new List<FeedbackData>();
        try
        {
            using (var connection = new MySqlConnection(connectionString))
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
        }
        catch (MySqlException sqlEx)
        {
            Console.WriteLine($"MySQL error: {sqlEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General error: {ex.Message}");
        }
        return feedbacks;
    }
}
