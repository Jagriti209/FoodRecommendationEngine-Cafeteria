using MySqlConnector;
using System;
using System.Collections.Generic;

public class NotificationRepository
{
    private readonly string connectionString;

    public NotificationRepository(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public void SaveNotification(Notification notification)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO Notification (message, date, notificationType) VALUES (@message, @date, @notificationType)";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@message", notification.Message);
                command.Parameters.AddWithValue("@date", notification.Date);
                command.Parameters.AddWithValue("@notificationType", notification.NotificationType);
                command.ExecuteNonQuery();
            }
        }
    }

    public List<Notification> GetNotifications()
    {
        var notifications = new List<Notification>();
        DateTime startDate = DateTime.Now;
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Notification WHERE Date >= NOW() - INTERVAL 1 DAY";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StartDate", startDate);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var notification = new Notification
                        {
                            Message = reader["Message"].ToString(),
                            Date = Convert.ToDateTime(reader["Date"]),
                            NotificationType = reader["NotificationType"].ToString()
                        };
                        notifications.Add(notification);
                    }
                }
            }
        }
        return notifications;
    }
}
