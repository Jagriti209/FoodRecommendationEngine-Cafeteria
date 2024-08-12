using ClientApp.Models;
using Newtonsoft.Json;
using System;

public class FeedbackService
{
    private readonly Client _client;

    public FeedbackService(Client client)
    {
        _client = client;
    }

    public FeedbackData[] GetFeedbackForMenu(int menuId)
    {
        try
        {
            CustomData requestData = new CustomData
            {
                Choice = "viewFeedback",
                Feedback = new FeedbackData
                {
                    MenuID = menuId
                }
            };

            string jsonData = JsonConvert.SerializeObject(requestData);
            string response = _client.SendRequestAndGetResponse(jsonData);
            FeedbackData[] feedback = JsonConvert.DeserializeObject<FeedbackData[]>(response);
            return feedback;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in FeedbackService.GetFeedbackForMenu: {ex.Message}");
            return null;
        }
    }
}
