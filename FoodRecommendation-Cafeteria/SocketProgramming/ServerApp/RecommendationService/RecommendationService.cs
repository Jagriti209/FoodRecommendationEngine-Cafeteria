public class RecommendationService
{
    private readonly MenuDataService _menuDataService;
    private readonly FeedbackDataService _feedbackDataService;
    private readonly SentimentAnalysisService _sentimentAnalysisService;
    private readonly RecommendationDataService _recommendationDataService;

    public RecommendationService()
    {
        _menuDataService = new MenuDataService();
        _feedbackDataService = new FeedbackDataService();
        _sentimentAnalysisService = new SentimentAnalysisService();
        _recommendationDataService = new RecommendationDataService();
    }

    List<MenuItemScore> menuItemScores = new List<MenuItemScore>();
    public List<MenuItemScore> GenerateRecommendation()
    {
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

            var topItems = menuItemScores.OrderByDescending(m => m.AverageScore).Take(5).ToList();
        }

        return menuItemScores;
    }

    public List<MenuItemScore> GetDiscardMenuItems()
    {
        var topItemsAscending = menuItemScores.OrderBy(m => m.AverageScore).Take(5).ToList();

        return topItemsAscending;
    }
    }
