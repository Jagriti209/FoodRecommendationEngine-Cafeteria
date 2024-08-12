using Newtonsoft.Json;
using ServerApp;
using System.Net.Sockets;

public static class MenuManager
{
    static RecommendationService recommendationService = new RecommendationService();

    public static void ViewMenu(NetworkStream stream)
    {
        try
        {
            var menuRepository = new MenuRepository();
            var menuService = new MenuService(menuRepository);

            var menuItems = menuService.GetMenuItems();
            var customData = new DisplayMenuItem
            {
                Items = menuItems
            };
            string responseDataJson = JsonConvert.SerializeObject(customData);
            ClientHandler.SendResponse(responseDataJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error viewing menu: {ex.Message}");
        }
    }

    public static void ViewMenuItemsToDiscarded(NetworkStream stream)
    {
        try
        {
            var discardedItems = recommendationService.GetDiscardMenuItems();
            string responseDataJson = JsonConvert.SerializeObject(discardedItems);
            ClientHandler.SendResponse(responseDataJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error viewing discarded menu items: {ex.Message}");
        }
    }
}