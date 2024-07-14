using System.Net.Sockets;
public class EmployeeRequestHandler
{
    public static void ProcessEmployeeAction(NetworkStream stream, CustomData requestData)
    {
        NotificationManager notificationManager = new NotificationManager();
        switch (requestData.Choice.ToLower())
        {
            case "getmenuitems":
                MenuManager.ViewMenu(stream);
                break;
            case "givefeedback":
                EmployeeMenuRepository.AddFeedback(stream, requestData.Feedback);
                break;
            case "viewfeedback":
                FeedbackManager.ViewFeedback(stream, requestData.Feedback.MenuID);
                break;
            case "logout":
                AuthenticationManager.LogUserLogout();
                break;
            case "viewnotifications":
                notificationManager.GetNotifications(stream);
                break;
            case "updateprofile":
                EmployeeMenuRepository.updateProfile(stream, requestData.UserData);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
}

