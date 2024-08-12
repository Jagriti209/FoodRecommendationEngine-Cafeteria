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
            case "getrolledoutmenuitems":
                EmployeeMenuRepository.GetRolledOutMenuItems(stream);
                break;
            case "viewfeedback":
                FeedbackManager.ViewFeedback(requestData.Feedback.MenuID);
                break;
            case "getdiscardedmenuitems":
                MenuManager.ViewMenuItemsToDiscarded(stream);
                break;
            case "discardmenuitem":
                ChefOperations.AddItemToDiscardedMenu(stream, requestData.MenuItem.MenuID);
                break;
            case "logout":
                Logout.LogUserLogout(requestData.UserData.UserID);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;

        }
    }
}
