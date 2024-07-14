using System.Collections.Generic;
using System.Linq;

public class RecommendationHandler
{
    public List<MenuItemScore> GetTopRecommendedItems(List<MenuItemScore> recommendations)
    {
        List<MenuItemScore> recommendedItems = new List<MenuItemScore>();

        foreach (string mealType in recommendations.Select(x => x.MealType).Distinct())
        {
            var topItems = recommendations
                .Where(item => item.MealType == mealType)
                .OrderByDescending(item => item.AverageScore)
                .Take(5)
                .ToList();

            recommendedItems.AddRange(topItems);
        }
        return recommendedItems;
    }

    public List<MenuItemScore> GetDiscardItems(List<MenuItemScore> recommendations)
    {
        List<MenuItemScore> discardItems = new List<MenuItemScore>();

        foreach (string mealType in recommendations.Select(x => x.MealType).Distinct())
        {
            var lowestItems = recommendations
                .Where(item => item.MealType == mealType && item.AverageScore < 2.5)
                .OrderBy(item => item.AverageScore)
                .ToList();

            discardItems.AddRange(lowestItems);
        }
        return discardItems;
    }
}
