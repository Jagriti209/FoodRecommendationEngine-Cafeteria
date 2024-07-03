using Newtonsoft.Json;

public class EmployeeMenuOperations
{
    private Client client;
    private MenuService menuService;
    private FeedbackService feedbackService;
    public EmployeeMenuOperations(Client client)
    {
        this.client = client;
        this.menuService = new MenuService(client);
        this.feedbackService = new FeedbackService(client);
    }

    public void ViewMenu()
    {
        Console.WriteLine("Requesting menu from server...");
        foreach (var item in menuService.GetMenuItems().Items)
        {
            Console.WriteLine($"menuId:{item.MenuID}, name: {item.itemName}, price: {item.Price}");
        }
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
            Feedback = new FeedbackData { Feedback = feedback, Rating = rating, MenuID = menuID }
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

}
