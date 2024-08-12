using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data;

namespace ServerApp
{
    [TestClass]
    public class MenuRepositoryTests
    {
        private Mock<IDbConnection> mockConnection;
        private Mock<IDbCommand> mockCommand;
        private Mock<IDataReader> mockReader;
        private MenuRepository menuRepository;

        [TestInitialize]
        public void Initialize()
        {
            mockConnection = new Mock<IDbConnection>();
            mockCommand = new Mock<IDbCommand>();
            mockReader = new Mock<IDataReader>();

            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.ExecuteReader()).Returns(mockReader.Object);
            mockCommand.SetupGet(cmd => cmd.CommandText).Returns("SELECT menuID, itemName, price, availability, mealType FROM Menu WHERE availability = 1");

            menuRepository = new MenuRepository();
        }

        [TestMethod]
        public void FetchMenuItemsReturnsItemsWhenDataIsPresent()
        {
            var mockData = new List<MenuItem>
            {
                new MenuItem { MenuID = 1, ItemName = "Aloo Paratha", Price = 40, Availability = true, MealType = "breakfast" },
                new MenuItem { MenuID = 2, ItemName = "Masala Dosa", Price = 50, Availability = true, MealType = "breakfast" }
            };

            mockReader.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(true)
                .Returns(false);

            mockReader.Setup(r => r.GetInt32(0)).Returns(mockData[0].MenuID);
            mockReader.Setup(r => r.GetString(1)).Returns(mockData[0].ItemName);
            mockReader.Setup(r => r.GetDecimal(2)).Returns(mockData[0].Price);
            mockReader.Setup(r => r.GetBoolean(3)).Returns(mockData[0].Availability);
            mockReader.Setup(r => r.GetString(4)).Returns(mockData[0].MealType);

            mockReader.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(true)
                .Returns(false);

            mockReader.Setup(r => r.GetInt32(0)).Returns(mockData[1].MenuID);
            mockReader.Setup(r => r.GetString(1)).Returns(mockData[1].ItemName);
            mockReader.Setup(r => r.GetDecimal(2)).Returns(mockData[1].Price);
            mockReader.Setup(r => r.GetBoolean(3)).Returns(mockData[1].Availability);
            mockReader.Setup(r => r.GetString(4)).Returns(mockData[1].MealType);

            var menuItems = menuRepository.FetchMenuItems();

            Assert.AreEqual("Aloo Paratha", menuItems[0].ItemName);
            Assert.AreEqual(40, menuItems[0].Price);
            Assert.AreEqual("Masala Dosa", menuItems[1].ItemName);
            Assert.AreEqual(50, menuItems[1].Price);
        }

        [TestMethod]
        public void FetchMenuItemsReturnsEmptyListWhenNoDataIsPresent()
        {
            mockReader.Setup(r => r.Read()).Returns(false);

            var menuItems = menuRepository.FetchMenuItems();

            Assert.AreEqual(0, menuItems.Count);
        }
    }
}
