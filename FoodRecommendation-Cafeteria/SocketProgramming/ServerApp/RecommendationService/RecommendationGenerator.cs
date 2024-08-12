using MySqlConnector;
using System;
using System.Collections.Generic;

public class RecommendationGenerator
{
    private readonly MenuDataService menuDataService;
    private readonly FeedbackDataService feedbackDataService;
    private readonly SentimentAnalysisService sentimentAnalysisService;

    public RecommendationGenerator()
    {
        menuDataService = new MenuDataService();
        feedbackDataService = new FeedbackDataService();
        sentimentAnalysisService = new SentimentAnalysisService();
    }

    public List<MenuItemScore> GenerateRecommendations()
    {
        List<MenuItemScore> menuItemScores = new List<MenuItemScore>();
        List<string> mealTypes = new List<string> { "breakfast", "lunch", "dinner" };

        try
        {
            foreach (string mealType in mealTypes)
            {
                List<MenuItem> menuItems = menuDataService.GetMenuItemsByMealType(mealType);

                foreach (var menuItem in menuItems)
                {
                    try
                    {
                        List<FeedbackData> feedbacks = feedbackDataService.GetFeedbacksByMenuId(menuItem.MenuID);
                        double averageScore = sentimentAnalysisService.CalculateAverageScore(feedbacks);

                        menuItemScores.Add(new MenuItemScore
                        {
                            MenuID = menuItem.MenuID,
                            AverageScore = averageScore,
                            ItemName = menuItem.ItemName,
                            MealType = menuItem.MealType
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing menu item ID {menuItem.MenuID}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating recommendations: {ex.Message}");
        }

        return menuItemScores;
    }
}
