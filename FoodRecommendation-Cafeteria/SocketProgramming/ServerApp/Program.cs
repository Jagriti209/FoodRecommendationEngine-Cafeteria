using System.Net;
using System.Net.Sockets;

class Program
{
    private const int port = 8080;
    private static string connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

    static void Main(string[] args)
    {
        TcpListener server = new TcpListener(IPAddress.Any, port);
        server.Start();
        Console.WriteLine("Server started...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Client connected...");

            var clientThread = new Thread(new ParameterizedThreadStart(ClientHandler.HandleClient));
            clientThread.Start(client);
        }
    }
}








/*
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using MySqlConnector;
using System.Threading;

class Program
{
    private const int port = 8080;
    private static string connectionString = "Server=localhost;Database=foodrecommendationenginedb;User ID=root;Password=root;";

    static void Main(string[] args)
    {
        TcpListener server = new TcpListener(IPAddress.Any, port);
        server.Start();
        Console.WriteLine("Server started...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
//add in authenticate method            Console.WriteLine("Client connected...");

            var clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
            clientThread.Start(client);
        }
    }

    private static void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;

        try
        {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string jsonData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                CustomData requestData = JsonConvert.DeserializeObject<CustomData>(jsonData);

                if (requestData != null)
                {
                    Console.WriteLine(requestData.Choice.ToLower());
                    switch (requestData.Choice.ToLower())
                    {
                        case "authenticate":
                            Console.WriteLine("yes");
                            Authenticate(stream, requestData);
                            break;
                        case "addmenuitem":
                            AddMenuItem(requestData.MenuItem);
                            break;
                        case "updatemenuitem":
                            UpdateMenuItem(requestData.MenuItem);
                            break;
                        case "deletemenuitem":
                            DeleteMenuItem(requestData.MenuItem);
                            break;
                        case "getmenuitems":
                            GetMenuItems(stream);
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid request data received.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
        finally
        {
            if (!client.Connected)
            {
                stream.Close();
                client.Close();
                Console.WriteLine("Client disconnected...");
            }
        }
    }

    private static void Authenticate(NetworkStream stream, CustomData requestData)
    {
        try
        {
            string roleType = AuthenticateUser(requestData.Name, requestData.Password);

            if (roleType != null)
            {
                Console.WriteLine($"Authentication successful. RoleType: {roleType}");
                requestData.RoleType = roleType;
                string responseDataJson = JsonConvert.SerializeObject(requestData);
                byte[] responseDataBytes = Encoding.ASCII.GetBytes(responseDataJson);
                stream.Write(responseDataBytes, 0, responseDataBytes.Length);
            }
            else
            {
                string errorMsg = "Authentication failed. Invalid username or password.";
                byte[] errorBytes = Encoding.ASCII.GetBytes(errorMsg);
                stream.Write(errorBytes, 0, errorBytes.Length);
                Console.WriteLine("Authentication failed. Invalid credentials.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during authentication: {ex.Message}");
        }
    }

    private static string AuthenticateUser(string username, string password)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = "SELECT RoleType FROM User WHERE Name = @username AND Password = @password";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    var result = command.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during authentication: {ex.Message}");
                return null;
            }
        }
    }

*//*    private static void ProcessClientChoice(NetworkStream stream, string roleType, CustomData clientChoiceData)
    {
        try
        {
            switch (clientChoiceData.Choice.ToLower())
            {
                case "addmenuitem":
                    AddMenuItem(clientChoiceData.MenuItem);
                    break;
                case "updatemenuitem":
                    UpdateMenuItem(clientChoiceData.MenuItem);
                    break;
                case "deletemenuitem":
                    DeleteMenuItem(clientChoiceData.MenuItem);
                    break;
                case "viewmenuitems":
                    getMenuItems(stream);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing client choice: {ex.Message}");
        }
    }*//*

    private static void GetMenuItems(NetworkStream stream)
    {
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
                            Console.WriteLine("yes",menuItem);
                            menuItems.Add(menuItem);

                        }
                        foreach (var item in menuItems)
                        {
                            Console.WriteLine($"Name: {item.Name}, Price: {item.Price}");
                            // Print other properties as needed
                        }
                        // Serialize menuItems to JSON and send to client
                        string responseDataJson = JsonConvert.SerializeObject(menuItems);
                        byte[] responseDataBytes = Encoding.UTF8.GetBytes(responseDataJson);
                        stream.Write(responseDataBytes, 0, responseDataBytes.Length);
                        stream.Flush(); // Ensure all data is sent
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
            stream.Flush(); // Ensure error message is sent
        }
    }

    private static void AddMenuItem(MenuItem menuItem)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = "INSERT INTO Menu (itemName, price, availability) VALUES (@itemName, @price, @availability)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@itemName", menuItem.Name);
                    command.Parameters.AddWithValue("@price", menuItem.Price);
                    command.Parameters.AddWithValue("@availability", menuItem.Availability);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) inserted.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding menu item: {ex.Message}");
            }
        }
    }

    private static void UpdateMenuItem(MenuItem menuItem)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = "UPDATE Menu SET price = @price, availability = @availability WHERE itemName = @itemName";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@price", menuItem.Price);
                    command.Parameters.AddWithValue("@availability", menuItem.Availability);
                    command.Parameters.AddWithValue("@itemName", menuItem.Name);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) updated.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating menu item: {ex.Message}");
            }
        }
    }

    private static void DeleteMenuItem(MenuItem menuItem)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = "DELETE FROM Menu WHERE itemName = @itemName";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@itemName", menuItem.Name);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) deleted.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting menu item: {ex.Message}");
            }
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
    public bool Availability { get; set; }
}
*/