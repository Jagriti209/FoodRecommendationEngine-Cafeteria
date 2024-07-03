using MySqlConnector;
using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class FeedbackRepository
{
    private readonly string connectionString;

    public FeedbackRepository(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public List<FeedbackData> FetchFeedback(int MenuId)
    {
        var feedbackList = new List<FeedbackData>();

        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT feedback, Rating, Date FROM Feedback WHERE MenuID = @MenuId and feedback is not null";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MenuId", MenuId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var feedback = new FeedbackData
                            {
                                Feedback = reader.GetString("feedback"),
                                Rating = reader.GetInt32("rating"),
                                Date = reader.GetDateTime("date")
                            };
                            feedbackList.Add(feedback);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching feedback: {ex.Message}");
        }

        return feedbackList;
    }
}
