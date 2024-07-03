public class AdminMenuOperations
{
    private Client client;
    private MenuService menuService;
    private FeedbackService feedbackService;
    public AdminMenuOperations(Client client)
    {
        this.client = client;
        this.menuService = new MenuService(client);
        this.feedbackService = new FeedbackService(client);
    }

    public void AddMenuItem()
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
            MenuItem = new MenuItem { itemName = itemName, Price = itemPrice, Available = itemAvailability }
        };

        client.SendDataToServer(data);
    }

    public void UpdateMenuItem()
    {
        ViewMenuItems();
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
            MenuItem = new MenuItem { itemName = itemNameToUpdate, Price = newItemPrice, Available = newItemAvailability }
        };

        client.SendDataToServer(data);
    }

    public void DeleteMenuItem()
    {
        ViewMenuItems();
        Console.WriteLine("Enter the name of the menu item to delete:");
        string itemNameToDelete = Console.ReadLine();

        CustomData data = new CustomData
        {
            Choice = "deleteMenuItem",
            MenuItem = new MenuItem { itemName = itemNameToDelete }
        };

        client.SendDataToServer(data);
    }

    public void ViewMenuItems()
    {
        var menuItems = menuService.GetMenuItems();
        Console.WriteLine("+----------+----------------------+---------------+");
        Console.WriteLine("| Menu ID  | Name                 | Price         |");
        Console.WriteLine("+----------+----------------------+---------------+");

        foreach (var item in menuItems.Items)
        {
            Console.WriteLine($"| {item.MenuID,-8} | {item.itemName,-20} | {item.Price.ToString("C"),-13} |");
            Console.WriteLine("+----------+----------------------+---------------+");
        }
    }

    public void ViewFeedback()
    {
        ViewMenuItems();
        Console.WriteLine("Enter menuID:");
        int menuID = int.Parse(Console.ReadLine());
        var feedbacks = feedbackService.GetFeedbackForMenu(menuID);
        Console.WriteLine("+-------------------------------+----------------+");
        Console.WriteLine("| Feedback                      | Rating         |");
        Console.WriteLine("+-------------------------------+----------------+");

        foreach (var item in feedbacks)
        {
            Console.WriteLine($"| {item.Feedback,-30} | {item.Rating,-14} |");
            Console.WriteLine("+-------------------------------+----------------+");
        }

    }
    public void ViewDiscardMenuItems()
    {
       var discardedItems =  menuService.GetDiscardedItems();
        Console.WriteLine("+----------------------+-----------------+----------------------+");
        Console.WriteLine("| MenuId               | MealType        | itemName             |");
        Console.WriteLine("+----------------------+-----------------+----------------------+");

        foreach (var item in discardedItems)
        {
            Console.WriteLine($"| {item.MenuID,-20} | {item.MealType,-15} | {item.itemName,-20} |");
            Console.WriteLine("+----------------------+-----------------+----------------------+");
        }
    }
}
