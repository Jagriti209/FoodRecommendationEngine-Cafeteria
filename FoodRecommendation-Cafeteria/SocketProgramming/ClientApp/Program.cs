using System;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

class Program
{
    private const string server = "127.0.0.1";
    private const int port = 8080;

    static void Main(string[] args)
    {
        try
        {            
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();
            string roleType = AuthenticateAndGetRole(name, password);

            if (roleType != null)
            {
                Console.WriteLine($"Authentication successful. Your role is: {roleType}");

                if (roleType.ToLower() == "admin")
                {
                    Console.WriteLine("Enter choice to perform the task");
                    Console.WriteLine("1. Add Menu Item");
                    Console.WriteLine("2. Update Menu Item");
                    Console.WriteLine("3. Delete Menu Item");
                    Console.WriteLine("4. Exit");
                    while (true)
                    {
                        Console.Write("Enter your choice: ");
                        string choice = Console.ReadLine().Trim().ToLower();

                        switch (choice)
                        {
                            case "1":
                                Console.WriteLine("Fill details to add menu items.");
                                PerformAddMenuItem();
                                break;
                            case "2":
                                Console.WriteLine("Fill details to update menu item.");
                                PerformUpdateMenuItem();
                                break;
                            case "3":
                                Console.WriteLine("Fill details to delete menu item.");
                                PerformDeleteMenuItem();
                                break;
                            case "4":
                                Console.WriteLine("Exiting...");
                                return;
                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("User role does not have admin privileges.");
                }
            }
            else
            {
                Console.WriteLine("Authentication failed. Invalid username or password.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
        }
    }

    private static string AuthenticateAndGetRole(string name, string password)
    {
        try
        {
            TcpClient client = new TcpClient(server, port);
            NetworkStream stream = client.GetStream();

            CustomData authData = new CustomData
            {
                Name = name,
                Password = password
            };

            string authJsonData = JsonConvert.SerializeObject(authData);
            byte[] authDataBytes = Encoding.ASCII.GetBytes(authJsonData);

            stream.Write(authDataBytes, 0, authDataBytes.Length);
            Console.WriteLine("Authentication data sent to server");

            byte[] authResponseBuffer = new byte[1024];
            int authBytesRead = stream.Read(authResponseBuffer, 0, authResponseBuffer.Length);
            string authResponse = Encoding.ASCII.GetString(authResponseBuffer, 0, authBytesRead);
            Console.WriteLine($"Authentication response: {authResponse}");

            CustomData responseData = JsonConvert.DeserializeObject<CustomData>(authResponse);
            string roleType = responseData.RoleType;

            stream.Close();
            client.Close();

            return roleType;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during authentication: {ex.Message}");
            return null;
        }
    }

    private static void PerformAddMenuItem()
    {
        
        Console.WriteLine("Enter details for the new menu item:");
        Console.Write("Name: ");
        string itemName = Console.ReadLine();
        Console.Write("Price: ");
        decimal itemPrice;
        while (!decimal.TryParse(Console.ReadLine(), out itemPrice))
        {
            Console.Write("Invalid input. Please enter a valid price: ");
        }
        Console.Write("Availability (True/False): ");
        bool itemAvailability;
        while (!bool.TryParse(Console.ReadLine(), out itemAvailability))
        {
            Console.Write("Invalid input. Please enter true or false: ");
        }

        
        CustomData data = new CustomData
        {
            Choice = "addMenuItem",
            MenuItem = new MenuItem { Name = itemName, Price = itemPrice, Available = itemAvailability }
        };

        SendDataToServer(data);
    }

    private static void PerformUpdateMenuItem()
    {
        
        Console.WriteLine("Enter details to update a menu item:");
        Console.Write("Name of item to update: ");
        string itemNameToUpdate = Console.ReadLine();
        Console.Write("New Price: ");
        decimal newItemPrice;
        while (!decimal.TryParse(Console.ReadLine(), out newItemPrice))
        {
            Console.Write("Invalid input. Please enter a valid price: ");
        }
        Console.Write("New Availability (True/False): ");
        bool newItemAvailability;
        while (!bool.TryParse(Console.ReadLine(), out newItemAvailability))
        {
            Console.Write("Invalid input. Please enter true or false: ");
        }

        
        CustomData data = new CustomData
        {
            Choice = "updateMenuItem",
            MenuItem = new MenuItem { Name = itemNameToUpdate, Price = newItemPrice, Available = newItemAvailability }
        };

        SendDataToServer(data);
    }

    private static void PerformDeleteMenuItem()
    {
        
        Console.WriteLine("Enter the name of the menu item to delete:");
        string itemNameToDelete = Console.ReadLine();

        
        CustomData data = new CustomData
        {
            Choice = "deleteMenuItem",
            MenuItem = new MenuItem { Name = itemNameToDelete }
        };

        SendDataToServer(data);
    }

    private static void SendDataToServer(CustomData data)
    {
        try
        {
            
            string jsonData = JsonConvert.SerializeObject(data);

            
            TcpClient client = new TcpClient(server, port);
            NetworkStream stream = client.GetStream();
            byte[] dataBytes = Encoding.ASCII.GetBytes(jsonData);
            stream.Write(dataBytes, 0, dataBytes.Length);
            Console.WriteLine("Data sent to server");

            
            byte[] responseBuffer = new byte[1024];
            int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
            string response = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead);
            Console.WriteLine($"Received: {response}");

            stream.Close();
            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }
}

public class CustomData
{
    public string Name { get; set; }
    public string Password { get; set; }
    public string RoleType { get; set; }
    public string Choice { get; set; } 
    public MenuItem MenuItem { get; set; } 
}

public class MenuItem
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public bool Available { get; set; }
}
