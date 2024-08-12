using System;
using System.Collections.Generic;
using System.Linq;

public class RecommendationHandler
{
    public List<MenuItemScore> GetTopRecommendedItems(List<MenuItemScore> recommendations)
    {
        List<MenuItemScore> recommendedItems = new List<MenuItemScore>();

        try
        {
            foreach (string mealType in recommendations.Select(x => x.MealType).Distinct())
            {
                try
                {
                    var topItems = recommendations
                        .Where(item => item.MealType == mealType)
                        .OrderByDescending(item => item.AverageScore)
                        .Take(5)
                        .ToList();

                    recommendedItems.AddRange(topItems);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing top recommended items for meal type {mealType}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving top recommended items: {ex.Message}");
        }

        return recommendedItems;
    }

    public List<MenuItemScore> GetDiscardItems(List<MenuItemScore> recommendations)
    {
        List<MenuItemScore> discardItems = new List<MenuItemScore>();

        try
        {
            foreach (string mealType in recommendations.Select(x => x.MealType).Distinct())
            {
                try
                {
                    var lowestItems = recommendations
                        .Where(item => item.MealType == mealType && item.AverageScore < 2.5)
                        .OrderBy(item => item.AverageScore)
                        .ToList();

                    discardItems.AddRange(lowestItems);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing discard items for meal type {mealType}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving discard items: {ex.Message}");
        }

        return discardItems;
    }
}
