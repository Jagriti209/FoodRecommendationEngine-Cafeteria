using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

public static class FeedbackManager
{
    private static readonly string connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

    public static void ViewFeedback(NetworkStream stream,int MenuId)
    {
        var feedbackRepository = new FeedbackRepository(connectionString);
        var feedbackService = new FeedbackService(feedbackRepository);

        var feedbackList = feedbackService.GetFeedback(MenuId);
        SendResponse(stream, feedbackList);
    }

    private static void SendResponse(NetworkStream stream, List<FeedbackData> feedbackList)
    {
        try
        {
            string responseDataJson = JsonConvert.SerializeObject(feedbackList);
            byte[] responseDataBytes = Encoding.UTF8.GetBytes(responseDataJson);
            stream.Write(responseDataBytes, 0, responseDataBytes.Length);
            stream.Flush();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending response: {ex.Message}");
            string errorMsg = "Error sending feedback response.";
            byte[] errorBytes = Encoding.UTF8.GetBytes(errorMsg);
            stream.Write(errorBytes, 0, errorBytes.Length);
            stream.Flush();
        }
    }
}
