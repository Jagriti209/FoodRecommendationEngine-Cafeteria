    using MySqlConnector;
    public class NotificationRepository
    {
        private readonly string connectionString;

        public NotificationRepository()
        {
            this.connectionString = Configuration.ConnectionString;
        }

        public void SaveNotification(Notification notification)
        {
            try
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
            catch (MySqlException ex)
            {
                Console.WriteLine($"Database error while saving notification: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while saving notification: {ex.Message}");
            }
        }

        public List<Notification> GetNotifications()
        {
            var notifications = new List<Notification>();
            DateTime startDate = DateTime.Now;

            try
            {
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
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Database error while retrieving notifications: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while retrieving notifications: {ex.Message}");
            }

            return notifications;
        }
    }
