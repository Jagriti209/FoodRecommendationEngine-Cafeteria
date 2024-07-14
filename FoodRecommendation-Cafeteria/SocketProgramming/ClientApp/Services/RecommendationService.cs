using Newtonsoft.Json;

public class RecommendationService
{
    private readonly Client _client;

    public RecommendationService(Client client)
    {
        _client = client;
    }

    public void GetRecommendation()
    {
        try
        {
            CustomData requestData = new CustomData
            {
                Choice = "viewRecommendation"
            };
            string jsonData = JsonConvert.SerializeObject(requestData);
            string response = _client.SendRequestAndGetResponse(jsonData);

            List<MenuItem> recommendations = JsonConvert.DeserializeObject<List<MenuItem>>(response);

            Console.WriteLine("\nTop 5 Recommendations for Each Meal Type:");
            Console.WriteLine("+----------------------+-----------------+----------------------+");
            Console.WriteLine("| MenuId               | MealType        | itemName             |");
            Console.WriteLine("+----------------------+-----------------+----------------------+");

            foreach (var recommendation in recommendations)
            {
                Console.WriteLine($"| {recommendation.MenuID,-20} | {recommendation.MealType,-20} | {recommendation.itemName,-20} |");
                Console.WriteLine("+----------------------+----------------------+");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }
}