using System;
using System.Collections.Generic;

public class NotificationService
{
    private readonly NotificationRepository notificationRepository;

    public NotificationService(NotificationRepository notificationRepository)
    {
        this.notificationRepository = notificationRepository;
    }

    public void SaveNotification(Notification notification)
    {
        notificationRepository.SaveNotification(notification);
    }

    public List<Notification> GetNotificationsForPast24Hours()
    {
        return notificationRepository.GetNotifications();
    }
}
