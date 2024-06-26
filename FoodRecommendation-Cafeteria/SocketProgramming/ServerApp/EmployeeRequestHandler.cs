using MySqlConnector;

public static class EmployeeRequestHandler
{
    private static string connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";
    public static void AddFeedback(FeedbackData feedback)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string getUserId = "SELECT userID INTO @userID FROM User WHERE name = 'Alice'";
                string getMenuId = "SELECT menuID INTO @menuID FROM Menu WHERE itemName = 'Pizza'";
                string insertFeedbackQuery = "INSERT INTO Feedback (userID, menuID, comment, rating, date) " +
                                         "VALUES (@userID, @menuID, @comment, @rating, CURDATE())";
                using (var command = new MySqlCommand(insertFeedbackQuery, connection))
                {
                    command.Parameters.AddWithValue("@userID", getUserId);
                    command.Parameters.AddWithValue("@menuID", getMenuId);
                    command.Parameters.AddWithValue("@comment", feedback.Comment);
                    command.Parameters.AddWithValue("@rating", feedback.Rating);
                    command.Parameters.AddWithValue("@date", feedback.Date);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) inserted.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding menu item: {ex.Message}");
            }
        }
    }

}
