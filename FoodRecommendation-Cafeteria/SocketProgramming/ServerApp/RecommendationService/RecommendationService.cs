public class RecommendationService
{
    private readonly RecommendationGenerator recommendationGenerator;
    private readonly RecommendationHandler recommendationFilter;

    public RecommendationService()
    {
        recommendationGenerator = new RecommendationGenerator();
        recommendationFilter = new RecommendationHandler();
    }

    public List<MenuItemScore> GetRecommendedMenuItems()
    {
        var recommendations = recommendationGenerator.GenerateRecommendations();
        return recommendationFilter.GetTopRecommendedItems(recommendations);
    }

    public List<MenuItemScore> GetDiscardMenuItems()
    {
        var recommendations = recommendationGenerator.GenerateRecommendations();
        return recommendationFilter.GetDiscardItems(recommendations);
    }
}
