using MySqlConnector;
using System.Collections.Generic;

public class RecommendationGenerator
{
    private readonly MenuDataService _menuDataService;
    private readonly FeedbackDataService _feedbackDataService;
    private readonly SentimentAnalysisService _sentimentAnalysisService;

    public RecommendationGenerator()
    {
        _menuDataService = new MenuDataService();
        _feedbackDataService = new FeedbackDataService();
        _sentimentAnalysisService = new SentimentAnalysisService();
    }

    public List<MenuItemScore> GenerateRecommendations()
    {
        List<MenuItemScore> menuItemScores = new List<MenuItemScore>();
        List<string> mealTypes = new List<string> { "breakfast", "lunch", "dinner" };

        foreach (string mealType in mealTypes)
        {
            List<MenuItem> menuItems = _menuDataService.GetMenuItemsByMealType(mealType);

            foreach (var menuItem in menuItems)
            {
                List<FeedbackData> feedbacks = _feedbackDataService.GetFeedbacksByMenuId(menuItem.MenuID);
                double averageScore = _sentimentAnalysisService.CalculateAverageScore(feedbacks);

                menuItemScores.Add(new MenuItemScore
                {
                    MenuID = menuItem.MenuID,
                    AverageScore = averageScore,
                    ItemName = menuItem.ItemName,
                    MealType = menuItem.MealType
                });
            }
        }
        return menuItemScores;
    }
}
