using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data;

namespace ServerApp.Tests
{
    [TestClass]
    public class NotificationRepositoryTests
    {
        private Mock<IDbConnection> mockConnection;
        private Mock<IDbCommand> mockCommand;
        private Mock<IDataReader> mockReader;
        private NotificationRepository notificationRepository;

        [TestInitialize]
        public void Initialize()
        {
            mockConnection = new Mock<IDbConnection>();
            mockCommand = new Mock<IDbCommand>();
            mockReader = new Mock<IDataReader>();

            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.ExecuteReader()).Returns(mockReader.Object);
            mockCommand.SetupGet(cmd => cmd.CommandText).Returns("SELECT * FROM Notification WHERE Date >= NOW() - INTERVAL 1 DAY");

            string connectionString = Configuration.ConnectionString;

            notificationRepository = new NotificationRepository();
        }

        [TestMethod]
        public void SaveNotification_SuccessfullySavesNotification()
        {
            var notification = new Notification
            {
                Message = "Test Message",
                Date = DateTime.Now,
                NotificationType = "Info"
            };

            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Verifiable();

            notificationRepository.SaveNotification(notification);

            mockCommand.Verify(cmd => cmd.ExecuteNonQuery(), Times.Once);
        }

        [TestMethod]
        public void GetNotifications_ReturnsNotifications_WhenDataIsPresent()
        {
            var mockNotificationData = new List<Notification>
            {
                new Notification { Message = "Test Message 1", Date = DateTime.Now, NotificationType = "Info" },
                new Notification { Message = "Test Message 2", Date = DateTime.Now, NotificationType = "Warning" }
            };

            mockReader.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(true)
                .Returns(false);

            mockReader.Setup(r => r["Message"]).Returns(mockData[0].Message);
            mockReader.Setup(r => r["Date"]).Returns(mockData[0].Date);
            mockReader.Setup(r => r["NotificationType"]).Returns(mockData[0].NotificationType);

            mockReader.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(false);

            mockReader.Setup(r => r["Message"]).Returns(mockData[1].Message);
            mockReader.Setup(r => r["Date"]).Returns(mockData[1].Date);
            mockReader.Setup(r => r["NotificationType"]).Returns(mockData[1].NotificationType);

            var notifications = notificationRepository.GetNotifications();

            Assert.AreEqual(2, notifications.Count);
            Assert.AreEqual("Test Message 1", notifications[0].Message);
            Assert.AreEqual("Test Message 2", notifications[1].Message);
        }

        [TestMethod]
        public void GetNotifications_ReturnsEmptyList_WhenNoDataIsPresent()
        {
            mockReader.Setup(r => r.Read()).Returns(false);

            var notifications = notificationRepository.GetNotifications();
        }

        [TestMethod]
        public void SaveNotification_HandlesException()
        {
            var notification = new Notification
            {
                Message = "Test Message",
                Date = DateTime.Now,
                NotificationType = "Info"
            };

            mockCommand.Setup(cmd => cmd.ExecuteNonQuery());

            notificationRepository.SaveNotification(notification);
        }

        [TestMethod]
        public void GetNotifications_HandlesException()
        {
            mockCommand.Setup(cmd => cmd.ExecuteReader()).Throws(new Exception("Database error"));

            var notifications = notificationRepository.GetNotifications();
        }
    }
}
