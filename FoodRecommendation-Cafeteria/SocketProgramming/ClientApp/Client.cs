using ClientApp.Models;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

public class Client
{
    public const string server = "127.0.0.1";
    public const int port = 8080;

    private TcpClient ConnectToServer()
    {
        try
        {
            TcpClient client = new TcpClient(server, port);
            return client;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception while connecting to server: {ex.Message}");
            return null;
        }
    }

    private string SendRequestAndGetResponse(string requestData)
    {
        try
        {
            TcpClient client = ConnectToServer();
            if (client == null)
                return null;

            NetworkStream stream = client.GetStream();
            byte[] dataBytes = Encoding.ASCII.GetBytes(requestData);
            stream.Write(dataBytes, 0, dataBytes.Length);

            byte[] responseBuffer = new byte[1024];
            int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
            string response = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead);

            stream.Close();
            //    client.Close();

            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during request/response: {ex.Message}");
            return null;
        }
    }

    public void SendDataToServer(CustomData data)
    {
        try
        {
            string jsonData = JsonConvert.SerializeObject(data);
            SendRequestAndGetResponse(jsonData);
            Console.WriteLine("Data sent to server");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
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
            string response = SendRequestAndGetResponse(jsonData);
            Console.WriteLine("Request for menu items sent to server");

            /*            CustomData menuItems = JsonConvert.DeserializeObject<CustomData>(response);
            
                        foreach (var item in menuItems)
                        {
                            Console.WriteLine($"name: {item.menuitem.name}, price: {item.menuitem.price}");
                        }*/
            DisplayMenuItem menuItems = JsonConvert.DeserializeObject<DisplayMenuItem>(response);
            foreach (var item in menuItems.Items)
            {
                Console.WriteLine($"name: {item.Name}, price: {item.Price}");
            }
            // Extract the menu items from the MenuItem property of CustomData
            // List<MenuItem> menuItems = customData.MenuItem;
           // Console.WriteLine("sf", menuItems);
            return menuItems;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return null;
        }
    }


    public FeedbackData[] getFeedbackAndRating()
    {
        try
        {
            CustomData requestData = new CustomData
            {
                Choice = "getFeedbackAndRating"
            };

            string jsonData = JsonConvert.SerializeObject(requestData);
            string response = SendRequestAndGetResponse(jsonData);
            Console.WriteLine("Request for menu items sent to server");

            FeedbackData[] feedback = JsonConvert.DeserializeObject<FeedbackData[]>(response);
            return feedback;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return null;
        }
    }
}
