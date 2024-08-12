using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;

namespace UnitTests
{
    [TestClass]
    public class FeedbackRepositoryTests
    {
        private Mock<IDbConnection> mockConnection;
        private Mock<IDbCommand> mockCommand;
        private Mock<IDataReader> mockReader;
        private FeedbackRepository feedbackRepository;  

        [TestInitialize]
        public void Initialize()
        {
            mockConnection = new Mock<IDbConnection>();
            mockCommand = new Mock<IDbCommand>();
            mockReader = new Mock<IDataReader>();

            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.ExecuteReader()).Returns(mockReader.Object);
            mockCommand.SetupGet(cmd => cmd.CommandText)
                        .Returns("SELECT feedback, Rating, Date FROM Feedback WHERE MenuID = @MenuId AND feedback IS NOT NULL");

            feedbackRepository = new FeedbackRepository("mockConnectionString");
        }

        [TestMethod]
        public void FetchFeedbackReturnsFeedbackDataWhenDataIsPresent()
        {
            var mockData = new List<FeedbackData>
            {
                new FeedbackData { Feedback = "Great!", Rating = 5, Date = DateTime.Now.AddDays(-1) },
                new FeedbackData { Feedback = "Good", Rating = 4, Date = DateTime.Now }
            };

            int readCallCount = 0;

            mockReader.Setup(r => r.Read())
                .Returns(() => readCallCount < mockData.Count)
                .Callback(() => readCallCount++);

            mockReader.Setup(r => r.GetString(0)).Returns(() => mockData[readCallCount - 1].Feedback);
            mockReader.Setup(r => r.GetInt32(1)).Returns(() => mockData[readCallCount - 1].Rating);
            mockReader.Setup(r => r.GetDateTime(2)).Returns(() => mockData[readCallCount - 1].Date);

            var feedbackList = feedbackRepository.FetchFeedback(1);

            Assert.AreEqual(2, feedbackList.Count);
            Assert.AreEqual("Great!", feedbackList[0].Feedback);
            Assert.AreEqual(5, feedbackList[0].Rating);
            Assert.AreEqual("Good", feedbackList[1].Feedback);
            Assert.AreEqual(4, feedbackList[1].Rating);
        }

        [TestMethod]
        public void FetchFeedbackReturnsEmptyListWhenNoDataIsPresent()
        {
            mockReader.Setup(r => r.Read()).Returns(false);

            var feedbackList = feedbackRepository.FetchFeedback(1);

            Assert.AreEqual(0, feedbackList.Count);
        }

        [TestCleanup]
        public void Cleanup()
        {
            mockConnection = null;
            mockCommand = null;
            mockReader = null;
        }
    }
}
