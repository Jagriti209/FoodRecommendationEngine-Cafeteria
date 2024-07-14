public class ChefMenuOperations
{
    private Client client;
    private MenuService menuService;
    private RecommendationService recommendationService;
    private FeedbackService feedbackService;

    public ChefMenuOperations(Client client)
    {
        this.client = client;
        this.menuService = new MenuService(client);
        this.recommendationService = new RecommendationService(client);
        this.feedbackService = new FeedbackService(client);
    }
    public void ViewMenu()
    {
        var menuItems = menuService.GetMenuItems();
        foreach (var item in menuItems.Items)
        {
            Console.WriteLine($"menuId:{item.MenuID}, name: {item.itemName}, price: {item.Price}");
        }
    }

    public void ViewRecommendation()
    {
        recommendationService.GetRecommendation();

    }

    public void CreateMenuForNextDay()
    {
        var menuItems = menuService.GetMenuItems();
        foreach (var item in menuItems.Items)
        {
            Console.WriteLine($"menuId:{item.MenuID}, MealType:{item.MealType}, name: {item.itemName}, price: {item.Price}");
        }
        bool continueInput = true;

        while (continueInput)
        {
            Console.WriteLine("Enter meal type (breakfast, lunch, dinner):");
            string mealType = Console.ReadLine();

            Console.WriteLine("Enter menu ID:");
            int menuID;
            while (!int.TryParse(Console.ReadLine(), out menuID))
            {
                Console.WriteLine("Invalid input. Please enter a valid menu ID:");
            }

            CustomData menuData = new CustomData
            {
                Choice = "createMenu",
                MenuItem = new MenuItem
                {
                    MenuID = menuID,
                    MealType = mealType

                },
                UserData = new UserData { UserID = AuthenticationService.UserId }
            };

            client.SendDataToServer(menuData);

            Console.WriteLine("Do you want to add another menu item? (yes/no):");
            string userInput = Console.ReadLine();
            if (userInput.Equals("no", StringComparison.OrdinalIgnoreCase))
            {
                continueInput = false;
            }
        }

        Console.WriteLine("Menu for next day created successfully.");
    }

    public void ViewFeedback()
    {
        ViewMenu();
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
        var discardedItems = menuService.GetDiscardedItems();
        Console.WriteLine("+----------------------+-----------------+----------------------+");
        Console.WriteLine("| MenuId               | MealType        | itemName             |");
        Console.WriteLine("+----------------------+-----------------+----------------------+");

        foreach (var item in discardedItems)
        {
            Console.WriteLine($"| {item.MenuID,-20} | {item.MealType,-15} | {item.itemName,-20} |");
            Console.WriteLine("+----------------------+-----------------+----------------------+");
        }
        GetUserChoice();
    }

    private void GetUserChoice()
    {
        Console.WriteLine("Do you want to delete any menu item or view detailed feedback?");
        Console.WriteLine("1. Delete a menu item");
        Console.WriteLine("2. View detailed feedback");
        Console.WriteLine("3. Exit");
        Console.Write("Enter your choice: ");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            switch (choice)
            {
                case 1:
                    DeleteDiscardedMenuItem();
                    break;
                case 2:
                    Console.WriteLine("Exiting.");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    GetUserChoice();
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a number.");
            GetUserChoice();
        }
    }

    private void DeleteDiscardedMenuItem()
    {
        Console.Write("Enter the MenuId of the item you want to delete: ");
        int itemIDToDelete = Convert.ToInt32(Console.ReadLine());

        CustomData data = new CustomData
        {
            Choice = "deleteMenuItem",
            MenuItem = new MenuItem { MenuID = itemIDToDelete }
        };

        client.SendDataToServer(data);
    }
}

