using Newtonsoft.Json;
using System.Net.Sockets;
public class EmployeeRequestHandler
{
    static RecommendationServiceForUser recommendationServiceForUser = new RecommendationServiceForUser();
    public static void ProcessEmployeeAction(NetworkStream stream, CustomData requestData)
    {
        NotificationHandler notificationManager = new NotificationHandler();
        switch (requestData.Choice.ToLower())
        {
            case "getmenuitems":
                MenuManager.ViewMenu(stream);
                break;
            case "getrolledoutmenuitems":
                EmployeeMenuRepository.GetRolledOutMenuItems(stream);
                break;
            case "givefeedback":
                EmployeeMenuRepository.AddFeedback(stream, requestData.Feedback);
                break;
            case "viewfeedback":
                FeedbackManager.ViewFeedback(requestData.Feedback.MenuID);
                break;
            case "logout":
                Logout.LogUserLogout(requestData.UserData.UserID);
                break;
            case "viewnotifications":
                notificationManager.GetNotifications(stream);
                break;
            case "viewrecommendation":
                recommendationServiceForUser.GenerateRecommendationForMe(stream, requestData.UserData.UserID);
                break;
            case "updateprofile":
                EmployeeMenuRepository.updateProfile(stream, requestData.UserData);
                break;
            case "viewdiscardedmenuitem":
                EmployeeMenuRepository.GetDiscardedMenuItems(stream);
                break;
            case "addfeedbacktodiscardedmenuitem":
                EmployeeMenuRepository.AddFeedbackToDiscardedMenuItem(stream,requestData.DiscardedMenuItemFeedback);
                break;
            case "voteforitem":
                EmployeeMenuRepository.AddVoteForProposedMenu(stream, requestData.MenuItem.MenuID);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                CustomData message = new CustomData
                {
                    Notification = new Notification { Message = "Invalid choice" }
                };
                string responseDataJson = JsonConvert.SerializeObject(message);
                ClientHandler.SendResponse(responseDataJson);
                break;
        }
    }
}

