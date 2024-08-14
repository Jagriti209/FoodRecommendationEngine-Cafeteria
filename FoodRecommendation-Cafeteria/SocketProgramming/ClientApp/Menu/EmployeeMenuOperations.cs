using Newtonsoft.Json;

public class EmployeeMenuOperations
{
    private Client client;
    private MenuService menuService;
    private FeedbackService feedbackService;
    private RecommendationService recommendationService;
    public EmployeeMenuOperations(Client client)
    {
        this.client = client;
        this.menuService = new MenuService(client);
        this.feedbackService = new FeedbackService(client);
        this.recommendationService = new RecommendationService(client);
    }

    public void ViewMenu()
    {
        Console.WriteLine("Requesting menu from server...");
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

    public void ViewRolledOutMenu()
    {
        Console.WriteLine("Requesting menu from server...");
        var menuItems = GetRolledOutMenuItems();
        Console.WriteLine("+----------+----------------------+----------------------+-----------------+");
        Console.WriteLine("| Menu ID  | Name                 | MealType             | Price           |");
        Console.WriteLine("+----------+----------------------+----------------------+-----------------+");

        foreach (var item in menuItems)
        {
            Console.WriteLine($"| {item.MenuID,-8} | {item.ItemName,-20} | {item.MealType,-20} | {item.Price,-15} |");
            Console.WriteLine("+----------+----------------------+---------------------+------------------+");
        }
        Console.WriteLine("Want to Vote for rolled out items now");
        Console.WriteLine("1. Yes");
        Console.WriteLine("2. No");
        int respones = int.Parse(Console.ReadLine());
        if(respones == 1)
        {
            VoteForMenuItems();
        }
    }

    public NextDayMenu[] GetRolledOutMenuItems()
    {
        try
        {
            CustomData requestData = new CustomData
            {
                Choice = "getRolledOutMenuItems"
            };

            string jsonData = JsonConvert.SerializeObject(requestData);
            string response = client.SendRequestAndGetResponse(jsonData);
            Console.WriteLine("Request for menu items sent to server");
            NextDayMenu[] menuItems = JsonConvert.DeserializeObject<NextDayMenu[]>(response);
            return menuItems;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return null;
        }
    }

    public void VoteForMenuItems()
    {
        Console.WriteLine("Enter the menuId on which you want to vote");
        int menuID = int.Parse(Console.ReadLine());
        CustomData sendData = new CustomData
        {
            Choice = "voteForItem",
            MenuItem = new MenuItem { MenuID = menuID }
        };
        client.SendDataToServer(sendData);
    }

    public void GiveFeedbackAndRating()
    {
        ViewMenu();

        Console.WriteLine("Enter the Menu ID for the item you want to provide feedback on:");
        int menuID;
        while (!int.TryParse(Console.ReadLine(), out menuID))
        {
            Console.WriteLine("Invalid input. Please enter a valid Menu ID:");
        }


        Console.WriteLine("Enter feedback:");
        string feedback = Console.ReadLine();

        Console.WriteLine("Enter rating (1-5):");
        int rating;
        while (!int.TryParse(Console.ReadLine(), out rating) || rating < 1 || rating > 5)
        {
            Console.WriteLine("Invalid input. Please enter a number between 1 and 5:");
        }

        CustomData requestData = new CustomData
        {
            Choice = "giveFeedback",
            Feedback = new FeedbackData { Feedback = feedback, Rating = rating, MenuID = menuID },
            UserData = new UserData { UserID = AuthenticationService.UserId }
        };

        client.SendDataToServer(requestData);
    }

    public void ViewFeedbackAndRating()
    {
        Console.WriteLine("Requesting feedback and ratings from server...");
        Console.WriteLine("Enter menuID:");
        int menuID = int.Parse(Console.ReadLine());
        foreach (var item in feedbackService.GetFeedbackForMenu(menuID))
        {
            Console.WriteLine($"feedback: {item.Feedback}, Rating :{item.Rating}");
        }

    }

    public void ViewNotifications()
    {
        try
        {
            CustomData requestData = new CustomData
            {
                Choice = "viewNotifications"
            };

            string jsonData = JsonConvert.SerializeObject(requestData);
            string response = client.SendRequestAndGetResponse(jsonData);

            Notification[] notifications = JsonConvert.DeserializeObject<Notification[]>(response);
            Console.WriteLine("+--------------------+---------------------+-------------------------------+");
            Console.WriteLine("| Notification Type  | Date                | Message                       |");
            Console.WriteLine("+--------------------+---------------------+-------------------------------+");
            foreach (var notification in notifications)
            {
                Console.WriteLine($"| {notification.NotificationType.PadRight(18)} | {notification.Date.ToString().PadRight(19)} | {notification.Message.PadRight(35)} |");
                Console.WriteLine("+--------------------+---------------------+-------------------------------+");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }

    public void GiveRecommendationForMe()
    {
        recommendationService.GetRecommendation();
    }
    public void updateProfile()
    {
        Console.WriteLine("Food Preference:");
        Console.WriteLine("1. Veg");
        Console.WriteLine("2. Non-Veg");
        Console.Write("Enter food Preference: ");
        int foodPreferenceChoice;
        string foodPreference;
        while (!int.TryParse(Console.ReadLine(), out foodPreferenceChoice) || (foodPreferenceChoice != 1 && foodPreferenceChoice != 2))
        {
            Console.Write("Invalid input. Please enter 1 for Veg or 2 for Non-Veg: ");
        }
        foodPreference = foodPreferenceChoice == 1 ? "Veg" : "Non-Veg";

        Console.WriteLine("Is it spicy? ");
        Console.WriteLine("1. True");
        Console.WriteLine("2. False");
        bool isSpicy;
        while (!bool.TryParse(Console.ReadLine(), out isSpicy))
        {
            Console.Write("Invalid input. Please enter true or false: ");
        }

        Console.WriteLine("Cuisine Preference:");
        Console.WriteLine("1. North Indian");
        Console.WriteLine("2. South Indian");
        Console.WriteLine("3. Chinese");
        Console.WriteLine("4. Other");
        Console.Write("Enter your choice: ");
        int cuisinePreferenceChoice;
        string cuisinePreference;
        while (!int.TryParse(Console.ReadLine(), out cuisinePreferenceChoice) || (cuisinePreferenceChoice < 1 || cuisinePreferenceChoice > 4))
        {
            Console.Write("Invalid input. Please enter 1, 2, 3, or 4: ");
        }
        cuisinePreference = cuisinePreferenceChoice switch
        {
            1 => "North Indian",
            2 => "South Indian",
            3 => "Chinese",
            4 => "Other",
            _ => throw new Exception("Invalid cuisine Preference")
        };
        CustomData data = new CustomData
        {
            Choice = "updateProfile",
            UserData = new UserData { UserID = AuthenticationService.UserId, FoodPreference = foodPreference, SpiceTolerant = isSpicy, CuisinePreference = cuisinePreference }
        };
        client.SendDataToServer(data);
    }

    public void addFeedBackToDiscardedItem()
    {
        var menuItems = GetDiscardedMenuItem();
        Console.WriteLine("+----------+");
        Console.WriteLine("| Menu ID  |");
        Console.WriteLine("+----------+");
        foreach (var item in menuItems)
        {
            Console.WriteLine($"| {item.MenuID,-8}|");
            Console.WriteLine("+---------+");
        }
        Console.WriteLine("Enter menuID to provide feedback on:");
        int menuID = int.Parse(Console.ReadLine());

        Console.WriteLine("What didn't you like about the food?");
        string dislikeReason = Console.ReadLine();
        Console.WriteLine("How would you like the dish to taste");
        string preferedTaste = Console.ReadLine();
        Console.WriteLine("Share your mom's recipe");
        string momsRecipe = Console.ReadLine();

        CustomData sendData = new CustomData
        {
            Choice = "addFeedbackToDiscardedMenuItem",
            DiscardedMenuItemFeedback = new DiscardedMenuItemFeedback {menuID = menuID,userID = AuthenticationService.UserId, dislikedReason = dislikeReason, preferedTaste = preferedTaste, momsRecipe = momsRecipe }
        };
        client.SendDataToServer(sendData);
    }

    public DiscardedMenuItem[] GetDiscardedMenuItem()
    {
        try
        {
            CustomData requestData = new CustomData
            {
                Choice = "viewDiscardedMenuItem",
            };
            string response = client.SendDataToServer(requestData);
            Console.WriteLine("Request for feedback data sent to server");

            DiscardedMenuItem[] menuItems = JsonConvert.DeserializeObject<DiscardedMenuItem[]>(response);
            return menuItems;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in FeedbackService.GetFeedbackForMenu: {ex.Message}");
            return null;
        }
    }
}
