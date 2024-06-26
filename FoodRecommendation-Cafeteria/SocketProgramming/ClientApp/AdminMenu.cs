using System;

public class AdminMenu
{
    private Client client;

    public AdminMenu(Client client)
    {
        this.client = client;
    }

    public void DisplayMenu()
    {
        Console.WriteLine("Admin actions:");
        Console.WriteLine("1. Add Menu Item");
        Console.WriteLine("2. Update Menu Item");
        Console.WriteLine("3. Delete Menu Item");
        Console.WriteLine("4. View Menu Items");
        Console.WriteLine("5. Logout");

        while (true)
        {
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine().Trim().ToLower();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Fill details to add menu items.");
                    AddMenuItem();
                    break;
                case "2":
                    Console.WriteLine("Fill details to update menu item.");
                    UpdateMenuItem();
                    break;
                case "3":
                    Console.WriteLine("Fill details to delete menu item.");
                    DeleteMenuItem();
                    break;
                case "4":
                    Console.WriteLine("Viewing menu items...");
                    ViewMenuItems();
                    break;
                case "5":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private void AddMenuItem()
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
      
        Console.WriteLine(itemAvailability);
        CustomData data = new CustomData
        {
            Choice = "addMenuItem",
            MenuItem = new MenuItem { Name = itemName, Price = itemPrice, Available = itemAvailability }
        };

       // DisplayMenu();
        client.SendDataToServer(data);
    }

    private void UpdateMenuItem()
    {
        // view menu
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
        Console.WriteLine(newItemAvailability);
        CustomData data = new CustomData
        {
            Choice = "updateMenuItem",
            MenuItem = new MenuItem { Name = itemNameToUpdate, Price = newItemPrice, Available = newItemAvailability }
        };

      //  DisplayMenu();
        client.SendDataToServer(data);
    }

    private void DeleteMenuItem()
    {
        Console.WriteLine("Enter the name of the menu item to delete:");
        string itemNameToDelete = Console.ReadLine();

        CustomData data = new CustomData
        {
            Choice = "deleteMenuItem",
            MenuItem = new MenuItem { Name = itemNameToDelete }
        };

     //   DisplayMenu();
        client.SendDataToServer(data);
    }

    private void ViewMenuItems()
    {
        client.GetMenuItems();
        foreach (var item in client.GetMenuItems().Items)
        {
            Console.WriteLine($"name: {item.Name}, price: {item.Price}");
        }
        // foreach (var item in client.GetMenuItems())
        //{
        //    Console.WriteLine($"Name: {item.MenuItem.Name}, Price: {item.MenuItem.Price}");
        //}
        DisplayMenu();
        //// loop to display all data
    }

}
