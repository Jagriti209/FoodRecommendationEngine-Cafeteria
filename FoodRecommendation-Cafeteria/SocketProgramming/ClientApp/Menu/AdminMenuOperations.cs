using Newtonsoft.Json;

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

        Console.WriteLine("Meal Type:");
        Console.WriteLine("1. Breakfast");
        Console.WriteLine("2. Lunch");
        Console.WriteLine("3. Dinner");
        Console.Write("Enter your choice: ");
        int mealTypeChoice;
        string mealType;
        while (!int.TryParse(Console.ReadLine(), out mealTypeChoice) || (mealTypeChoice < 1 || mealTypeChoice > 3))
        {
            Console.Write("Invalid input. Please enter 1, 2 or 3: ");
        }
        mealType = mealTypeChoice switch
        {
            1 => "Breakfast",
            2 => "Lunch",
            3 => "Dinner",
            _ => throw new Exception("Invalid Meal type")
        };

        Console.WriteLine("Food Type:");
        Console.WriteLine("1. Veg");
        Console.WriteLine("2. Non-Veg");
        Console.Write("Enter food type: ");
        int foodTypeChoice;
        string foodType;
        while (!int.TryParse(Console.ReadLine(), out foodTypeChoice) || (foodTypeChoice != 1 && foodTypeChoice != 2))
        {
            Console.Write("Invalid input. Please enter 1 for Veg or 2 for Non-Veg: ");
        }
        foodType = foodTypeChoice == 1 ? "Veg" : "Non-Veg";

        Console.WriteLine("Is it spicy? ");
        Console.WriteLine("1. True");
        Console.WriteLine("2. False");
        bool isSpicy;
        while (!bool.TryParse(Console.ReadLine(), out isSpicy))
        {
            Console.Write("Invalid input. Please enter true or false: ");
        }

        Console.WriteLine("Cuisine Type:");
        Console.WriteLine("1. North Indian");
        Console.WriteLine("2. South Indian");
        Console.WriteLine("3. Chinese");
        Console.WriteLine("4. Other");
        Console.Write("Enter your choice: ");
        int cuisineTypeChoice;
        string cuisineType;
        while (!int.TryParse(Console.ReadLine(), out cuisineTypeChoice) || (cuisineTypeChoice < 1 || cuisineTypeChoice > 4))
        {
            Console.Write("Invalid input. Please enter 1, 2, 3, or 4: ");
        }
        cuisineType = cuisineTypeChoice switch
        {
            1 => "North Indian",
            2 => "South Indian",
            3 => "Chinese",
            4 => "Other",
            _ => throw new Exception("Invalid cuisine type")
        };

        Console.WriteLine("Is it sweet? ");
        Console.WriteLine("1. True");
        Console.WriteLine("2. False");
        bool isSweet;
        while (!bool.TryParse(Console.ReadLine(), out isSweet))
        {
            Console.Write("Invalid input. Please enter true or false: ");
        }

        CustomData data = new CustomData
        {
            Choice = "addMenuItem",
            MenuItem = new MenuItem
            {
                itemName = itemName,
                Price = itemPrice,
                Availability = itemAvailability,
                MealType = mealType,
                FoodType = foodType,
                IsSpicy = isSpicy,
                CuisineType = cuisineType,
                IsSweet = isSweet
            }
        };

        var response = client.SendDataToServer(data);
        CustomData responseData = JsonConvert.DeserializeObject<CustomData>(response);
        Console.WriteLine($"{responseData.Notification.Message}");
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
            MenuItem = new MenuItem { itemName = itemNameToUpdate, Price = newItemPrice, Availability = newItemAvailability }
        };

        var response = client.SendDataToServer(data);
        CustomData responseData = JsonConvert.DeserializeObject<CustomData>(response);
        Console.WriteLine($"{responseData.Notification.Message}");
    }

    public void DeleteMenuItem()
    {
        ViewMenuItems();
        Console.WriteLine("Enter the menuID of the menu item to delete:");
        int itemIDToDelete = Convert.ToInt32(Console.ReadLine());

        CustomData data = new CustomData
        {
            Choice = "deleteMenuItem",
            MenuItem = new MenuItem { MenuID = itemIDToDelete }
        };

        var response = client.SendDataToServer(data);
        CustomData responseData = JsonConvert.DeserializeObject<CustomData>(response);
        Console.WriteLine($"{responseData.Notification.Message}");
    }

    public void ViewMenuItems()
    {
        var menuItems = menuService.GetMenuItems();
        Console.WriteLine("+----------+----------------------+-----------------+");
        Console.WriteLine("| Menu ID  | Name                 | Price           |");
        Console.WriteLine("+----------+----------------------+-----------------+");

        foreach (var item in menuItems.Items)
        {
            Console.WriteLine($"| {item.MenuID,-8} | {item.itemName,-20} | {item.Price,-15} |");
            Console.WriteLine("+----------+----------------------+-----------------+");
        }
    }

    public void ViewFeedback()
    {
        ViewMenuItems();
        Console.WriteLine("Enter menuID:");
        int menuID = int.Parse(Console.ReadLine());
        var feedbacks = feedbackService.GetFeedbackForMenu(menuID);
        Console.WriteLine("+--------------------------------------+----------------+");
        Console.WriteLine("| Feedback                      | Rating                |");
        Console.WriteLine("+--------------------------------------+----------------+");

        foreach (var item in feedbacks)
        {
            Console.WriteLine($"| {item.Feedback,-40} | {item.Rating,-14} |");
            Console.WriteLine("+-------------------------------+----------------+");
        }

    }
    public void ViewDiscardMenuItems()
    {
        var discardedItems = menuService.GetDiscardedItems();
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
