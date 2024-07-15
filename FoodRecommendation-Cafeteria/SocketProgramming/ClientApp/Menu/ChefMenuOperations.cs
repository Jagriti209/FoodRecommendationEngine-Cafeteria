public class ChefMenuOperations
{
    private Client client;
    private MenuService menuService;
    private RecommendationService recommendationService;
    private FeedbackService feedbackService;
    private EmployeeMenuOperations employeeMenuOperations;
    public ChefMenuOperations(Client client)
    {
        this.client = client;
        this.menuService = new MenuService(client);
        this.recommendationService = new RecommendationService(client);
        this.feedbackService = new FeedbackService(client);
        this.employeeMenuOperations = new EmployeeMenuOperations(client);
    }
    public void ViewMenu()
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

    public void ViewRecommendation()
    {
        recommendationService.GetRecommendation();

    }

    public void CreateMenuForNextDay()
    {
        ViewMenu();
        bool continueInput = true;

        while (continueInput)
        {
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
                    MenuID = menuID
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
        Console.WriteLine("+-----------------------------------------+----------------+");
        Console.WriteLine("| Feedback                                | Rating         |");
        Console.WriteLine("+-----------------------------------------+----------------+");

        foreach (var item in feedbacks)
        {
            Console.WriteLine($"| {item.Feedback,-40} | {item.Rating,-14} |");
            Console.WriteLine("+-----------------------------------------+----------------+");
        }
    }
    public void ViewDiscardMenuItems()
    {
        var discardedItems = menuService.GetDiscardedItems();
        Console.WriteLine("+----------------------+-----------------+---------------------+");
        Console.WriteLine("| MenuId               | MealType        | itemName             |");
        Console.WriteLine("+----------------------+-----------------+---------------------+");

        foreach (var item in discardedItems)
        {
            Console.WriteLine($"| {item.MenuID,-20} | {item.MealType,-15} | {item.itemName,-20} |");
            Console.WriteLine("+----------------------+-----------------+----------------------+");
        }
        GetUserChoice();
    }

    private void GetUserChoice()
    {
        Console.WriteLine("Do you want to discard any menu item or view detailed feedback?");
        Console.WriteLine("1. Discard a menu item");
        Console.WriteLine("2. Get detailed feedback");
        Console.WriteLine("3. Exit");
        Console.Write("Enter your choice: ");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            switch (choice)
            {
                case 1:
                    DiscardMenuItem();
                    break;
                case 2:
                    GetDetailedFeedback();
                    break;
                case 3:
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

    private void DiscardMenuItem()
    {
        Console.Write("Enter the MenuId of the item you want to discard: ");
        int itemIdToDiscard = Convert.ToInt32(Console.ReadLine());
        CustomData data = new CustomData
        {
            Choice = "discardMenuItem",
            MenuItem = new MenuItem { MenuID = itemIdToDiscard }
        };

        string response = client.SendDataToServer(data);
        Console.WriteLine(response);

    }

    public void GetDetailedFeedback()
    {
        Console.WriteLine("Enter itemName on which detailed feedback needed");
        string ItemName = Console.ReadLine();
        CustomData data = new CustomData
        {
            Notification = { Message = $"Chef has discarded {ItemName} from the menu. please provide a detailed feedback on it" },
            MenuItem = {itemName = ItemName },
            Choice = "addFeedbackToDiscardedMenuItem"
        };
        client.SendDataToServer(data);
    }

    public void ViewRolledOutMenu()
    {
        var menuItems = employeeMenuOperations.GetRolledOutMenuItems();
        Console.WriteLine("+----------+----------------------+----------------------+-----------------+-----------------+");
        Console.WriteLine("| Menu ID  | Name                 | MealType             | Price           | Votes           |");
        Console.WriteLine("+----------+----------------------+----------------------+-----------------+-----------------+");

        foreach (var item in menuItems)
        {
            Console.WriteLine($"| {item.MenuID,-8} | {item.ItemName,-20} | {item.MealType,-20} | {item.Price,-15} | {item.votes,-15} |");
            Console.WriteLine("+----------+----------------------+---------------------+------------------+-----------------+");
        }
    }
}

