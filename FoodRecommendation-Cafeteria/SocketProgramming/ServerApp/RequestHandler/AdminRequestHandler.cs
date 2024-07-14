using System.Net.Sockets;

public class AdminRequestHandler 
{
    public static void ProcessAdminAction(NetworkStream stream, CustomData requestData)
    {
        switch (requestData.Choice.ToLower())
        {
            case "addmenuitem":
                AdminOperations.AddMenuItem(stream, requestData.MenuItem);
                break;
            case "updatemenuitem":
                AdminOperations.UpdateMenuItem(stream, requestData.MenuItem);
                break;
            case "deletemenuitem":
                AdminOperations.DeleteMenuItem(stream, requestData.MenuItem.MenuID);
                break;
            case "getmenuitems":
                MenuManager.ViewMenu(stream);
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