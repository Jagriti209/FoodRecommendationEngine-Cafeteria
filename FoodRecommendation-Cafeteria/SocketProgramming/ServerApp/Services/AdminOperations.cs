using MySqlConnector;
using System.Net.Sockets;

public class AdminOperations
{
    static NotificationManager notificationManager = new NotificationManager();
    public static void AddMenuItem(NetworkStream stream, MenuItem menuItem)
    {
        try
        {
            AdminMenuRepository.AddMenuItem(menuItem);
            notificationManager.SaveNotifications(new Notification
            {
                Message = $"{menuItem.ItemName} has been added to menu",
                Date = DateTime.Now,
                NotificationType = "MenuItemAdded"
            });
            CustomData message = new CustomData
            {
                Message = "{menuItem.ItemName } added successfully"
            };
            ClientHandler.SendResponse(stream, message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding menu item: {ex.Message}");
            CustomData message = new CustomData
            {
                Message = $"Error adding menu item: {ex.Message}"
            };
            ClientHandler.SendResponse(stream, message);
        }
    }

    public static void UpdateMenuItem(NetworkStream stream, MenuItem menuItem)
    {
        try
        {
            AdminMenuRepository.UpdateMenuItem(menuItem);
            notificationManager.SaveNotifications(new Notification
            {
                Message = $"{menuItem.ItemName} has been updated in the menu",
                Date = DateTime.Now,
                NotificationType = "MenuItemUpdated"
            });
            CustomData message = new CustomData
            {
                Message = $"{menuItem.ItemName} updated successfully"
            };
            ClientHandler.SendResponse(stream, message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating menu item: {ex.Message}");
            CustomData message = new CustomData
            {
                Message = $"Error updating menu item: {ex.Message}"
            };
            ClientHandler.SendResponse(stream, message);
        }
    }

    public static void DeleteMenuItem(NetworkStream stream, int itemID)
    {
        try
        {
            var itemName = AdminMenuRepository.DeleteMenuItem(itemID);
            notificationManager.SaveNotifications(new Notification
            {
                Message = $"{itemName} has been deleted from the menu",
                Date = DateTime.Now,
                NotificationType = "MenuItemDeleted"
            });
            CustomData message = new CustomData
            {
                Message = $"{itemName} deleted successfully"
            };
            ClientHandler.SendResponse(stream, message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting menu item: {ex.Message}");
            CustomData message = new CustomData
            {
                Message = $"Error deleting menu item: {ex.Message}"
            };
            ClientHandler.SendResponse(stream, message);
        }
    }
}
