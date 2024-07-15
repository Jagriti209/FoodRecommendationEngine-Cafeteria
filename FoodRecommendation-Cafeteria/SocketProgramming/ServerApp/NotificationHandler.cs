using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

public class NotificationHandler
{
    private readonly NotificationService notificationService;

    public NotificationHandler()
    {
        this.notificationService = new NotificationService();
    }

    public void SaveNotifications(Notification notification)
    {
        notificationService.SaveNotification(notification);
    }

    public void GetNotifications(NetworkStream stream)
    {
        try
        {
            List<Notification> notifications = notificationService.GetNotificationsForPast24Hours();
            string responseDataJson = JsonConvert.SerializeObject(notifications);
            ClientHandler.SendResponse(responseDataJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching notifications: {ex.Message}");
        }
    }
}
