using System.Net.Sockets;

public static class OperationsManager
{
    public static void ProcessAction(NetworkStream stream, string roleType, CustomData requestData)
    {
        switch (roleType.ToLower())
        {
            case "admin":
                ProcessAdminAction(stream, requestData);
                break;
/*            case "chef":
                ProcessChefAction(stream, requestData);
                break;*/
            case "employee":
                ProcessEmployeeAction(stream, requestData);
                break;
            default:
                Console.WriteLine("Invalid role type.");
                break;
        }

    }


    public static void ProcessAdminAction(NetworkStream stream, CustomData requestData)
    {
        switch (requestData.Choice.ToLower())
        {
            case "addmenuitem":
                AdminRequestHandler.AddMenuItem(requestData.MenuItem);
                break;
            case "updatemenuitem":
                AdminRequestHandler.UpdateMenuItem(requestData.MenuItem);
                break;
            case "deletemenuitem":
                AdminRequestHandler.DeleteMenuItem(requestData.MenuItem.Name);
                break;
            case "getmenuitems":
                MenuManager.ViewMenu(stream);
                break;
            case "viewfeedback":
                FeedbackManager.ViewFeedback(stream);
                break;
            case "viewfeedbackandrating":
                FeedbackManager.ViewFeedbackAndRating(stream);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }


  /*  public static void ProcessChefAction(NetworkStream stream, CustomData requestData)
    {
        switch (requestData.Choice.ToLower())
        {
            case "viewmenu":
                MenuManager.ViewMenu(stream);
                break;
            case "viewrecommendation":
                RecommendationManager.ViewRecommendation(stream);
                break;
            case "createmenu":
                MenuManager.CreateMenuForNextDay(stream);
                break;
            case "sendnotifications":
                NotificationManager.SendNotifications(stream);
                break;
            case "generatereport":
                ReportManager.GenerateReport(stream);
                break;
            case "generaterecommendation":
                RecommendationManager.GenerateRecommendation(stream);
                break;
            case "viewfeedback":
                FeedbackManager.ViewFeedback(stream);
                break;
            case "logout":
                LogoutManager.Logout(stream);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;

        }
    }*/

    public static void ProcessEmployeeAction(NetworkStream stream, CustomData requestData)
    {
        switch (requestData.Choice.ToLower())
        {
            case "getmenuitems":
                MenuManager.ViewMenu(stream);
                break;
            case "givefeedback":
                EmployeeRequestHandler.AddFeedback(requestData.Feedback);
                break;
            case "viewfeedback":
                FeedbackManager.ViewFeedback(stream);
                break;
            case "logout":
                AuthenticationManager.Logout(stream);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
}
