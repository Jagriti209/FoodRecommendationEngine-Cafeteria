/*using MySqlConnector;
using Newtonsoft.Json;
using System.Data;

public class RecommendationManager
{
    static string connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";
    public static List<MenuItemScore> GenerateRecommendation()
    {
        string jsonFilePath = "C:\\Users\\jagriti.anchalia\\OneDrive - InTimeTec Visionsoft Pvt. Ltd.,\\Desktop\\sentiemntWords.json";
        string jsonData = File.ReadAllText(jsonFilePath);
        List<WordScore> wordScores = JsonConvert.DeserializeObject<List<WordScore>>(jsonData);

        List<string> mealTypes = new List<string> { "breakfast", "lunch", "dinner" };
        List<MenuItemScore> menu = new List<MenuItemScore>();
        List<MenuItemScore> menuItemScores = new List<MenuItemScore>();
        foreach (string mealType in mealTypes)
        {
            List<MenuItem> menuItems = GetMenuItemsByMealType(mealType);
            Console.WriteLine($"Menu items for {mealType}:\n");
            foreach (var menuItem in menuItems)
            {
                List<FeedbackData> feedbacks = GetFeedbacksByMenuId(menuItem.MenuID);
                double averageScore = CalculateAverageScore(feedbacks, wordScores);
                if (averageScore > 0)
                {
                    menuItemScores.Add(new MenuItemScore { MenuID = menuItem.MenuID, AverageScore = averageScore, ItemName = menuItem.ItemName, MealType = menuItem.MealType });
                }
            }
            var topItems = menuItemScores.OrderByDescending(m => m.AverageScore).Take(5).ToList();
            foreach (var menuItem in topItems)
            {
                Console.WriteLine($"{menuItem.ItemName}");
            }
        }
            *//*            foreach (var menuItemScore in menuItemScores)
                        {
                            InsertRecommendation(menuItemScore, mealType);
                        }*//*
        return menuItemScores;
    }


    public static void InsertRecommendation(MenuItemScore menuItemScore, string mealType)
    {
        using (var connection = new MySqlConnection(connectionString))
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
    public static List<MenuItem> GetMenuItemsByMealType(string mealType)
    {
        List<MenuItem> menuItems = new List<MenuItem>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT menuID, itemName FROM Menu WHERE mealType = @mealType";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@mealType", mealType);
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

    public static List<FeedbackData> GetFeedbacksByMenuId(int menuID)
    {
        List<FeedbackData> feedbacks = new List<FeedbackData>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT feedback,rating FROM Feedback WHERE menuID = @menuID and feedback is not null";
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
                            Rating = reader.GetInt32("rating"),
                        });
                    }
                }
            }
        }
        return feedbacks;
    }

    public static double CalculateAverageScore(List<FeedbackData> feedbacks, List<WordScore> wordScores)
    {
        double totalScore = 0;
        int count = 0;

        foreach (var feedback in feedbacks)
        {
            double feedbackScore = CalculateFeedbackScore(feedback.Feedback, wordScores);
            if (feedbackScore != 0)
            {
                totalScore += feedbackScore;
                count++;
            }

        }
        return count > 0 ? totalScore / count : 0;
    }

    public static double CalculateFeedbackScore(string comment, List<WordScore> wordScores)
    {
        string[] words = comment.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        double score = 0;

        foreach (string word in words)
        {
            WordScore match = wordScores.Find(ws => ws.Word.Equals(word, StringComparison.OrdinalIgnoreCase));
            if (match != null)
            {
                score += match.Score;
            }
        }
        return score;
    }
}


*/