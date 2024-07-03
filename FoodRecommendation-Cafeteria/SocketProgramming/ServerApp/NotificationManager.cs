using MySqlConnector;
using Newtonsoft.Json;
using System.IO;
using System.Net.Sockets;
using System.Text;
public class NotificationManager
{
    private static string connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";
    NotificationRepository notificationRepository = new NotificationRepository(connectionString);
    public void SaveNotifications(Notification notification)
    {
        notificationRepository.SaveNotification(notification);
    }

    public void GetNotifications(NetworkStream stream)
    {
        try
        {
            List<Notification> notifications = notificationRepository.GetNotifications();
            string responseDataJson = JsonConvert.SerializeObject(notifications);
            byte[] responseDataBytes = Encoding.ASCII.GetBytes(responseDataJson);
            stream.Write(responseDataBytes, 0, responseDataBytes.Length);
            stream.Flush();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching notifications: {ex.Message}");
        }

    }
}