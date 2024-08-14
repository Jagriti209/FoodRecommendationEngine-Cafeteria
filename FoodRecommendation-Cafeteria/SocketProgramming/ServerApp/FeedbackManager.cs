using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

public static class FeedbackManager
{
    private static readonly string connectionString = Configuration.ConnectionString;

    public static void ViewFeedback(int MenuId)
    {
        var feedbackRepository = new FeedbackRepository(connectionString);
        var feedbackService = new FeedbackService(feedbackRepository);

        var feedbackList = feedbackService.GetFeedback(MenuId);
        string responseDataJson = JsonConvert.SerializeObject(feedbackList);
        ClientHandler.SendResponse(responseDataJson);
    }
}
