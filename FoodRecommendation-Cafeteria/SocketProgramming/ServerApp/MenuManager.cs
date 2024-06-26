using MySqlConnector;
using Newtonsoft.Json;
using ServerApp;
using System.Net.Sockets;
using System.Text;

public static class MenuManager
{
    private static string connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

    public static void ViewMenu(NetworkStream stream)
    {
        Console.WriteLine("ayya");
        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT itemName, price, availability FROM Menu";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var menuItems = new List<MenuItem>();

                        while (reader.Read())
                        {
                            var menuItem = new MenuItem
                            {
                                Name = reader.GetString("itemName"),
                                Price = reader.GetDecimal("price"),
                                Availability = reader.GetBoolean("availability")
                            };
                            menuItems.Add(menuItem);

                        }
                        DisplayMenuItem customData = new();
                        customData.Items = menuItems;
                        //Console.WriteLine("yes", menuItems);
                        /*foreach (var item in menuItems)
                        {
                            Console.WriteLine($"Name: {item.Name}, Price: {item.Price}");
                        }*/
                        string responseDataJson = JsonConvert.SerializeObject(customData);
                         byte[] responseDataBytes = Encoding.ASCII.GetBytes(responseDataJson);
                        stream.Write(responseDataBytes, 0, responseDataBytes.Length);
                        stream.Flush();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching menu items: {ex.Message}");

            string errorMsg = "Error fetching menu items.";
            byte[] errorBytes = Encoding.UTF8.GetBytes(errorMsg);
            stream.Write(errorBytes, 0, errorBytes.Length);
            stream.Flush();
        }
    }
}
