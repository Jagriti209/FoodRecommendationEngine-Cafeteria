using ClientApp.Models;
using Newtonsoft.Json;

public class MenuService
{
    private readonly Client _client;

    public MenuService(Client client)
    {
        _client = client;
    }

    public DisplayMenuItem GetMenuItems()
    {
        try
        {
            CustomData requestData = new CustomData
            {
                Choice = "getMenuItems"
            };

            string jsonData = JsonConvert.SerializeObject(requestData);
            string response = _client.SendRequestAndGetResponse(jsonData);
            Console.WriteLine("Request for menu items sent to server");
            DisplayMenuItem menuItems = JsonConvert.DeserializeObject<DisplayMenuItem>(response);
            return menuItems;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return null;
        }
    }

    public MenuItem[] GetDiscardedItems()
    {
        try
        {
            CustomData requestData = new CustomData
            {
                Choice = "getDiscardedMenuItems"
            };

            string jsonData = JsonConvert.SerializeObject(requestData);
            string response = _client.SendRequestAndGetResponse(jsonData);
            Console.WriteLine("Request for menu items sent to server");
            MenuItem[] menuItems = JsonConvert.DeserializeObject<MenuItem[]>(response);
            return menuItems;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return null;
        }
    }
}