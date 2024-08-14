using Newtonsoft.Json;
using System.Net.Sockets;

public class AdminOperations
{
    static NotificationHandler notificationHandler = new NotificationHandler();
    public static void AddMenuItem(NetworkStream stream, MenuItem menuItem)
    {
        try
        {
            string itemName = AdminMenuRepository.AddMenuItem(menuItem);
            notificationHandler.SaveNotifications(new Notification
            {
                Message = $"{itemName} has been added to menu",
                Date = DateTime.Now,
                NotificationType = "MenuItemAdded"
            });
            CustomData message = new CustomData
            {
                Notification = new Notification { Message = $"{itemName} added successfully" }
            };
            string responseDataJson = JsonConvert.SerializeObject(message);
            ClientHandler.SendResponse(responseDataJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding menu item: {ex.Message}");
            CustomData message = new CustomData
            {
                Notification = new Notification { Message = $"Error adding menu item: {ex.Message}" }
            };
            string responseDataJson = JsonConvert.SerializeObject(message);
            ClientHandler.SendResponse(responseDataJson);
        }
    }

    public static void UpdateMenuItem(NetworkStream stream, MenuItem menuItem)
    {
        try
        {
            AdminMenuRepository.UpdateMenuItem(menuItem);
            notificationHandler.SaveNotifications(new Notification
            {
                Message = $"{menuItem.ItemName} has been updated in the menu",
                Date = DateTime.Now,
                NotificationType = "MenuItemUpdated"
            });
            CustomData message = new CustomData
            {
                Notification = new Notification { Message = $"{menuItem.ItemName} updated successfully" }
            };
            string responseDataJson = JsonConvert.SerializeObject(message);
            ClientHandler.SendResponse(responseDataJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating menu item: {ex.Message}");
            CustomData message = new CustomData
            {
                Notification = new Notification { Message = $"Error updating menu item: {ex.Message}" }
            };
            string responseDataJson = JsonConvert.SerializeObject(message);
            ClientHandler.SendResponse(responseDataJson);
        }
    }

    public static void DeleteMenuItem(NetworkStream stream, int itemID)
    {
        try
        {
            var itemName = AdminMenuRepository.DeleteMenuItem(itemID);
            notificationHandler.SaveNotifications(new Notification
            {
                Message = $"{itemName} has been deleted from the menu",
                Date = DateTime.Now,
                NotificationType = "MenuItemDeleted"
            });
            CustomData message = new CustomData
            {
                Notification = new Notification { Message = $"{itemName} deleted successfully" }
            };
            string responseDataJson = JsonConvert.SerializeObject(message);
            ClientHandler.SendResponse(responseDataJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting menu item: {ex.Message}");
            CustomData message = new CustomData
            {
                Notification = new Notification { Message = $"Error deleting menu item: {ex.Message}" }
            };
            string responseDataJson = JsonConvert.SerializeObject(message);
            ClientHandler.SendResponse(responseDataJson);
        }
    }
}
