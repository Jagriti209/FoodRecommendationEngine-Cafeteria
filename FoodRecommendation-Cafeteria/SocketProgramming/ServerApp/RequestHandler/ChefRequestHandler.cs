using System.Net.Sockets;
public class ChefRequestHandler
{
    public static void ProcessChefAction(NetworkStream stream, CustomData requestData)
    {
        switch (requestData.Choice.ToLower())
        {
            case "getmenuitems":
                MenuManager.ViewMenu(stream);
                break;
            case "viewrecommendation":
                ChefOperations.ViewRecommendation(stream);
                break;
            case "createmenu":
                ChefOperations.CreateMenuForNextDay(stream, requestData.MenuItem);
                break;
            case "viewfeedback":
                FeedbackManager.ViewFeedback(stream, requestData.Feedback.MenuID);
                break;
            case "getdiscardedmenuitems":
                MenuManager.ViewDiscardedMenuItems(stream);
                break;
            case "logout":
                AuthenticationManager.LogUserLogout();
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;

        }
    }
}
