using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using ServerApp;
using System.IO;

public static class MenuManager
{
    private static readonly string connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";
    static RecommendationService recommendationService = new RecommendationService();
    
    public static void ViewMenu(NetworkStream stream)
    {
        var menuRepository = new MenuRepository(connectionString);
        var menuService = new MenuService(menuRepository);

        var menuItems = menuService.GetMenuItems();
        SendResponse(stream, menuItems);
    }

    public static void ViewDiscardedMenuItems(NetworkStream stream)
    {
        var discardedItems = recommendationService.GetDiscardMenuItems();
        string responseDataJson = JsonConvert.SerializeObject(discardedItems);
        byte[] responseDataBytes = Encoding.ASCII.GetBytes(responseDataJson);
        stream.Write(responseDataBytes, 0, responseDataBytes.Length);
        stream.Flush();
    }
    private static void SendResponse(NetworkStream stream, List<MenuItem> menuItems)
    {
        try
        {
            var customData = new DisplayMenuItem
            {
                Items = menuItems
            };
            string responseDataJson = JsonConvert.SerializeObject(customData);
            byte[] responseDataBytes = Encoding.ASCII.GetBytes(responseDataJson);
            stream.Write(responseDataBytes, 0, responseDataBytes.Length);
            stream.Flush();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending response: {ex.Message}");
            string errorMsg = "Error sending menu items response.";
            byte[] errorBytes = Encoding.UTF8.GetBytes(errorMsg);
            stream.Write(errorBytes, 0, errorBytes.Length);
            stream.Flush();
        }
    }
}
