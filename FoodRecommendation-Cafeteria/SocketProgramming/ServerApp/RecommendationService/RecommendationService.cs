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

    List<MenuItemScore> menuItemsList = new List<MenuItemScore>();
    public List<MenuItemScore> GenerateRecommendation()
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
        foreach (var menuItem in menuItemScores)
        {
            Console.WriteLine(menuItem.MenuID + " name " + menuItem.ItemName);
        }
        return menuItemScores;
    }

    /*    public List<MenuItemScore> GetRecommendedMenuItems()
        {
            var topItemsDescending = GenerateRecommendation();
            List<string> mealTypes = new List<string> { "breakfast", "lunch", "dinner" };
            return topItemsDescending;
        }*/

    public List<MenuItemScore> GetRecommendedMenuItems()
    {
        var topItemsDescending = GenerateRecommendation();
        List<string> mealTypes = new List<string> { "breakfast", "lunch", "dinner" };
        List<MenuItemScore> recommendedItems = new List<MenuItemScore>();

        foreach (string mealType in mealTypes)
        {
            List<MenuItemScore> topItems = topItemsDescending
                .Where(item => item.MealType == mealType)
                .OrderByDescending(item => item.AverageScore)
                .Take(5)
                .ToList();

            recommendedItems.AddRange(topItems);
        }
        foreach (var menuItem in recommendedItems)
        {
            Console.WriteLine(menuItem.MenuID + "name" + menuItem.ItemName);
        }
        // Return the list of recommended menu items
        return recommendedItems;
    }


    public List<MenuItemScore> GetDiscardMenuItems()
    {
        var topItemsAscending = GenerateRecommendation();
        List<string> mealTypes = new List<string> { "breakfast", "lunch", "dinner" };

        List<MenuItemScore> recommendedItems = new List<MenuItemScore>();
        /// >2 items add
        foreach (string mealType in mealTypes)
        {
            List<MenuItemScore> topItems = topItemsAscending
                .Where(item => item.MealType == mealType && item.AverageScore < 2.5)
                .OrderBy(item => item.AverageScore)
                .ToList();

            recommendedItems.AddRange(topItems);
        }
        foreach (var menuItem in recommendedItems)
        {
            Console.WriteLine(menuItem.MenuID + " name " + menuItem.ItemName);
        }
        // Return the list of recommended menu items
        return recommendedItems;
    }
}


/*menuItemScores = menuItemScores.OrderByDescending(m => m.AverageScore).Take(5).ToList();
menuItemsList.AddRange(menuItemScores);

        }
        foreach (var menuItem in menuItemsList)
{
    Console.WriteLine(menuItem.MenuID + "name" + menuItem.ItemName);
}
Console.WriteLine("-----------");
foreach (var menuItem in menuItemScores)
{
    Console.WriteLine(menuItem.MenuID + "name" + menuItem.ItemName);
}*/