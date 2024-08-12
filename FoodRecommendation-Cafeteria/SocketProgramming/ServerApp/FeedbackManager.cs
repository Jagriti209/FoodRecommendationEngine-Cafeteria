using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

public static class FeedbackManager
{
    private static readonly string connectionString = Configuration.ConnectionString;

    public static void ViewFeedback(int MenuId)
    {
        try
        {
            var feedbackRepository = new FeedbackRepository(connectionString);
            var feedbackService = new FeedbackService(feedbackRepository);

            var feedbackList = feedbackService.GetFeedback(MenuId);
            string responseDataJson = JsonConvert.SerializeObject(feedbackList);
            ClientHandler.SendResponse(responseDataJson);
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"JSON serialization error: {jsonEx.Message}");
        }
        catch (SocketException socketEx)
        {
            Console.WriteLine($"Socket error: {socketEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General error: {ex.Message}");
        }
    }
}
