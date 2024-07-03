using MySqlConnector;
using System;
using System.IO;
using System.Net.Sockets;

public static class EmployeeMenuRepository
{
    private static string connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

    public static void AddFeedback(NetworkStream stream,FeedbackData feedback)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string getUserId = "SELECT userID INTO @userID FROM User WHERE name = '@feedback.UserID'";
                string insertFeedbackQuery = "INSERT INTO Feedback (menuID, feedback, rating, date) " +
                                         "VALUES (@menuID, @feedback, @rating, CURDATE())";
                using (var command = new MySqlCommand(insertFeedbackQuery, connection))
                {
                    command.Parameters.AddWithValue("@menuID", feedback.MenuID);
                    command.Parameters.AddWithValue("@feedback", feedback.Feedback);
                    command.Parameters.AddWithValue("@rating", feedback.Rating);
                    command.Parameters.AddWithValue("@date", feedback.Date);
                }
                CustomData message = new CustomData
                {
                    Message = $"feedback updated successfully"
                };
                ClientHandler.SendResponse(stream,message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding menu item: {ex.Message}");
            }
        }
    }
}
