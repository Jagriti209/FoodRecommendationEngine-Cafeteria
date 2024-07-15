using MySqlConnector;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

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
                    Notification = new Notification{ Message = $"feedback updated successfully" }
                };
                Console.WriteLine("wrote");
                string responseDataJson = JsonConvert.SerializeObject(message);
                ClientHandler.SendResponse(responseDataJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding feedback on menu item: {ex.Message}");
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

    public static List<MenuItem> GetDiscardedMenuItems(NetworkStream stream)
    {
        var menuItems = new List<MenuItem>();

        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT menuID FROM DiscardedMenuItem where discardedDate >= NOW() - INTERVAL 1 DAY";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var menuItem = new MenuItem
                            {
                                MenuID = reader.GetInt32("menuID")
                            };
                            menuItems.Add(menuItem);
                        }
                    }
                }
            }
            string responseDataJson = JsonConvert.SerializeObject(menuItems);
            byte[] responseDataBytes = Encoding.ASCII.GetBytes(responseDataJson);
            stream.Write(responseDataBytes, 0, responseDataBytes.Length);
            stream.Flush();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching menu items: {ex.Message}");
        }

        return menuItems;
    }

    public static void AddFeedbackToDiscardedMenuItem(NetworkStream stream, DiscardedMenuItemFeedback discardedMenuItemFeedback)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string insertFeedbackQuery = "INSERT INTO DiscardedMenuItemFeedback (menuID,userID,momsRecipe,dislikedReason,feedbackAddedDate,preferedTaste) " +
                                         "VALUES (@menuID, @userID, @momsRecipe, @dislikedReason,@feedbackAddedDate,@preferedTaste)";
                using (var command = new MySqlCommand(insertFeedbackQuery, connection))
                {
                    command.Parameters.AddWithValue("@menuID", discardedMenuItemFeedback.menuID);
                    command.Parameters.AddWithValue("@userID", discardedMenuItemFeedback.userID);
                    command.Parameters.AddWithValue("@momsRecipe", discardedMenuItemFeedback.momsRecipe);
                    command.Parameters.AddWithValue("@preferedTaste", discardedMenuItemFeedback.preferedTaste);
                    command.Parameters.AddWithValue("@feedbackAddedDate", discardedMenuItemFeedback.feedbackAddedDate);
                    command.Parameters.AddWithValue("@dislikedReason", discardedMenuItemFeedback.dislikedReason);

                    command.ExecuteNonQuery();
                }

                CustomData message = new CustomData
                {
                    Notification = new Notification { Message = $"feedback updated successfully" }
                };
                string responseDataJson = JsonConvert.SerializeObject(message);
                ClientHandler.SendResponse(responseDataJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding menu item: {ex.Message}");
            }
        }

    }

    public static void GetRolledOutMenuItems(NetworkStream stream)
    {
        var menuItems = new List<NextDayMenu>();

        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT m.menuID,m.itemName, m.price,m.mealType,ndm.votes FROM menu m JOIN nextdaymenu ndm ON m.menuID = ndm.menuID;";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var menuItem = new NextDayMenu
                            {
                                MenuID = reader.GetInt32("menuID"),
                                ItemName = reader.GetString("itemName"),
                                Price = reader.GetDecimal("price"),
                                MealType = reader.GetString("mealType"),
                                votes = reader.GetInt32("votes")
                            };
                            menuItems.Add(menuItem);
                        }
                    }
                }
            }
            string responseDataJson = JsonConvert.SerializeObject(menuItems);
            ClientHandler.SendResponse(responseDataJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching menu items: {ex.Message}");
        }
    }

    public static void AddVoteForProposedMenu(NetworkStream stream, int menuID)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string insertFeedbackQuery = "UPDATE nextdaymenu SET votes = votes + 1 WHERE menuID = @menuID";
                using (var command = new MySqlCommand(insertFeedbackQuery, connection))
                {
                    command.Parameters.AddWithValue("@menuID", menuID);

                    command.ExecuteNonQuery();
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) updated.");
                }
                CustomData message = new CustomData
                {
                    Notification = new Notification { Message = $"vote updated successfully" }
                };
                string responseDataJson = JsonConvert.SerializeObject(message);
                ClientHandler.SendResponse(responseDataJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding vote: {ex.Message}");
            }
        }
    }
}
