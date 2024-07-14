using MySqlConnector;
using System.Net.Sockets;

public static class EmployeeMenuRepository
{
    private static string connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

    public static void AddFeedback(NetworkStream stream, FeedbackData feedback)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string insertFeedbackQuery = "INSERT INTO Feedback (menuID, feedback, rating, date) " +
                                         "VALUES (@menuID, @feedback, @rating, CURDATE())";
                using (var command = new MySqlCommand(insertFeedbackQuery, connection))
                {
                    command.Parameters.AddWithValue("@menuID", feedback.MenuID);
                    command.Parameters.AddWithValue("@feedback", feedback.Feedback);
                    command.Parameters.AddWithValue("@rating", feedback.Rating);
                    command.Parameters.AddWithValue("@date", feedback.Date);

                    command.ExecuteNonQuery();
                }

                CustomData message = new CustomData
                {
                    Notification = { Message = $"feedback updated successfully"}
                };
                Console.WriteLine("wrote");
                ClientHandler.SendResponse(stream, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding menu item: {ex.Message}");
            }
        }
    }

    public static void updateProfile(NetworkStream stream, UserData userData)
    {
        string connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

        try
        {

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string updateQuery = "UPDATE User SET FoodPreference = @FoodPreference, SpiceTolerant = @SpiceTolerant, CuisinePreference = @CuisinePreference WHERE UserID = @userID";
                using (var command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userData.UserID);
                    command.Parameters.AddWithValue("@FoodPreference", userData.FoodPreference);
                    command.Parameters.AddWithValue("@SpiceTolerant", userData.SpiceTolerant ? 1 : 0);
                    command.Parameters.AddWithValue("@CuisinePreference", userData.CuisinePreference);

                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) inserted.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating profile: {ex.Message}");
            throw;
        }
    }
}
