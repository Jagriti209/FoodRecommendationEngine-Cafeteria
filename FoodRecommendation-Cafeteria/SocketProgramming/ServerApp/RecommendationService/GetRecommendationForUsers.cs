using MySqlConnector;
using Newtonsoft.Json;
using System.IO;
using System.Net.Sockets;
using System.Text;

public class RecommendationServiceForUser
{
    private readonly FeedbackDataService _feedbackDataService;
    private readonly SentimentAnalysisService _sentimentAnalysisService;
    private readonly string connectionString = Configuration.ConnectionString;
    public RecommendationServiceForUser()
    {
        _feedbackDataService = new FeedbackDataService();
        _sentimentAnalysisService = new SentimentAnalysisService();
    }
    public void GenerateRecommendationForMe(NetworkStream stream, int userId)
    {
        List<MenuItemScore> recommendedMenu = new List<MenuItemScore>();
        var userDetails = GetUserPreferences(userId);
        List<string> mealTypes = new List<string> { "breakfast", "lunch", "dinner" };
        foreach (string mealType in mealTypes)
        {
            List<MenuItem> menuItems = GetMenuItemsByUserPreferences(mealType, userDetails);

            foreach (var menuItem in menuItems)
            {
                List<FeedbackData> feedbacks = _feedbackDataService.GetFeedbacksByMenuId(menuItem.MenuID);
                double averageScore = _sentimentAnalysisService.CalculateAverageScore(feedbacks);


                recommendedMenu.Add(new MenuItemScore
                {
                    MenuID = menuItem.MenuID,
                    AverageScore = averageScore,
                    ItemName = menuItem.ItemName,
                    MealType = menuItem.MealType
                });

            }
        }
        string responseDataJson = JsonConvert.SerializeObject(recommendedMenu);
        ClientHandler.SendResponse(responseDataJson);
    }
    public UserData GetUserPreferences(int userId)
    {
        UserData userData = new UserData();
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = "SELECT UserID, Name, Password, roleType, FoodPreference, SpiceTolerant, CuisinePreference FROM User WHERE UserID = @userId";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userID", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userData = (new UserData
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Name = reader["Name"].ToString(),
                                Password = reader["Password"].ToString(),
                                UserRole = reader["roleType"].ToString(),
                                FoodPreference = reader["FoodPreference"].ToString(),
                                SpiceTolerant = Convert.ToBoolean(reader["SpiceTolerant"]),
                                CuisinePreference = reader["CuisinePreference"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user with ID {userId}: {ex.Message}");
            }
        }

        return userData;
    }

    public List<MenuItem> GetMenuItemsByUserPreferences(string mealType, UserData userDetails)
    {
        List<MenuItem> menuItems = new List<MenuItem>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT menuID, itemName FROM Menu WHERE MealType = @MealType and foodType = @foodType and IsSpicy = @IsSpicy and cuisineType = @cuisineType and availability = @availability ";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MealType", mealType);
                command.Parameters.AddWithValue("@foodType", userDetails.FoodPreference);
                command.Parameters.AddWithValue("@IsSpicy", userDetails.SpiceTolerant);
                command.Parameters.AddWithValue("@cuisineType", userDetails.CuisinePreference);
                command.Parameters.AddWithValue("@availability", 1);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        menuItems.Add(new MenuItem
                        {
                            MenuID = reader.GetInt32("menuID"),
                            ItemName = reader.GetString("itemName"),
                            MealType = mealType
                        });
                    }
                }
            }
        }

        return menuItems;
    }
}