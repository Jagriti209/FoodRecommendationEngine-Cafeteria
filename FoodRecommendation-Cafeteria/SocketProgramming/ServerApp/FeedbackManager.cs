using MySqlConnector;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

public static class FeedbackManager
{
    private static string connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

    public static void ViewFeedback(NetworkStream stream)
    {
        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT f.feedbackID, u.userID, u.name AS userName, f.menuID, f.comment, f.rating, f.date FROM Feedback f JOIN User u ON f.userID = u.userID;";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var Feedback = new List<FeedbackData>();

                        while (reader.Read())
                        {
                            var feedback = new FeedbackData
                            {
                                Comment = reader.GetString("comment"),
                                Rating = reader.GetInt32("rating"),
                                Date = reader.GetDateTime("date")
                            };
                            Feedback.Add(feedback);

                        }
                        foreach (var data in Feedback)
                        {
                            Console.WriteLine($"Comment: {data.Comment}, Rating: {data.Rating}");
                        }
                        string responseDataJson = JsonConvert.SerializeObject(Feedback);
                        byte[] responseDataBytes = Encoding.UTF8.GetBytes(responseDataJson);
                        stream.Write(responseDataBytes, 0, responseDataBytes.Length);
                        stream.Flush();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching Feedback: {ex.Message}");

            string errorMsg = "Error fetching menu Feedback.";
            byte[] errorBytes = Encoding.UTF8.GetBytes(errorMsg);
            stream.Write(errorBytes, 0, errorBytes.Length);
            stream.Flush();
        }
    }

    public static void ViewFeedbackAndRating(NetworkStream stream)
    {
        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Feedback, Rating FROM Feedback";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var feedbackList = new List<FeedbackData>();
                        while (reader.Read())
                        {
                            FeedbackData feedback = new FeedbackData
                            {
                                Comment = reader.GetString("comment"),
                                Rating = reader.GetInt32("rating")
                            };
                            feedbackList.Add(feedback);
                        }
                        string responseDataJson = JsonConvert.SerializeObject(feedbackList);
                        byte[] responseDataBytes = Encoding.UTF8.GetBytes(responseDataJson);
                        stream.Write(responseDataBytes, 0, responseDataBytes.Length);
                        stream.Flush();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching feedback and rating: {ex.Message}");

            string errorMsg = "Error fetching feedback and rating.";
            byte[] errorBytes = Encoding.UTF8.GetBytes(errorMsg);
            stream.Write(errorBytes, 0, errorBytes.Length);
            stream.Flush();
        }
    }
}
