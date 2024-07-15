using System.Net.Sockets;
using Newtonsoft.Json;
using System.Text;

public static class ChefOperations
{
    static RecommendationService recommendationService = new RecommendationService();
    static NotificationHandler notificationManager = new NotificationHandler();
    public static void ViewRecommendation(NetworkStream stream)
    {
        var responseData = recommendationService.GetRecommendedMenuItems();
        string responseDataJson = JsonConvert.SerializeObject(responseData);
        byte[] responseDataBytes = Encoding.UTF8.GetBytes(responseDataJson);
        stream.Write(responseDataBytes, 0, responseDataBytes.Length);
        stream.Flush();
    }

    public static void CreateMenuForNextDay(NetworkStream stream, MenuItem menuData)
    {
        try
        {
            ChefMenuRepository.CreateMenuForNextDay(menuData);
            CustomData message = new CustomData
            {
                Notification = new Notification { Message = $"{menuData.MenuID} added successfully to Menu" }
            };
            string responseDataJson = JsonConvert.SerializeObject(message);
            ClientHandler.SendResponse(responseDataJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating menu for next day: {ex.Message}");
            CustomData message = new CustomData
            {
                Notification = new Notification { Message = $"Error creating menu for next day: {ex.Message}" }
            };
            string responseDataJson = JsonConvert.SerializeObject(message);
            ClientHandler.SendResponse(responseDataJson);
        }
    }

    public static void AddItemToDiscardedMenu(NetworkStream stream, int menuId)
    {
            string itemName = ChefMenuRepository.AddItemToDiscardedMenu(menuId);
        try
        {
            notificationManager.SaveNotifications(new Notification
            {
                Message = $"{itemName} has been added to discarded menu",
                Date = DateTime.Now,
                NotificationType = "DiscardedMenuItem"
            });
            CustomData message = new CustomData
            {
                Notification = new Notification { Message = $"{itemName} has been added successfully to Discared Menu" }
            };
            string responseDataJson = JsonConvert.SerializeObject(message);
            ClientHandler.SendResponse(responseDataJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while adding menu item to discarded list: {ex.Message}");
            CustomData message = new CustomData
            {
                Notification = new Notification { Message = $"error adding {itemName} to Discarded Menu" }
            };
            string responseDataJson = JsonConvert.SerializeObject(message);
            ClientHandler.SendResponse(responseDataJson);
        }
    }

}
