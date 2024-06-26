public class EmployeeMenu
{
    private Client client;

    public EmployeeMenu(Client client)
    {
        this.client = client;
    }

    public void DisplayMenu()
    {
        while (true)
        {
            ShowMenuOptions();
            string choice = GetUserChoice();

            if (choice == "4")
            {
                Console.WriteLine("Logging out...");
                break;
            }

            ProcessChoice(choice);
        }
    }

    private void ShowMenuOptions()
    {
        Console.WriteLine("Employee actions:");
        Console.WriteLine("1. View Menu");
        Console.WriteLine("2. Give Feedback & Rating");
        Console.WriteLine("3. View Feedback and Rating");
        Console.WriteLine("4. Logout");
    }

    private string GetUserChoice()
    {
        Console.Write("Enter your choice: ");
        return Console.ReadLine().Trim().ToLower();
    }

    private void ProcessChoice(string choice)
    {
        switch (choice)
        {
            case "1":
                ViewMenu();
                break;
            case "2":
                GiveFeedbackAndRating();
                break;
            /*            case "3":
                            ViewFeedbackAndRating();
                            break;*/
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }

    private void ViewMenu()
    {
        Console.WriteLine("Requesting menu from server...");
        client.GetMenuItems();
        foreach (var item in client.GetMenuItems().Items)
        {
            Console.WriteLine($"name: {item.Name}, price: {item.Price}");
        }
        /*        var responseData = client.GetMenuItems();
                if (responseData != null)
                {
                    Console.WriteLine("Received menu items:");
                    //foreach (CustomData item in responseData)
                    //{
                    //    Console.WriteLine($"Name: {item.MenuItem.Name}, Price: {item.MenuItem.Price}, Available: {item.MenuItem.Available}");
                    //}
                }
                else
                {
                    Console.WriteLine("Failed to retrieve menu items from server.");
                }*/
        ShowMenuOptions();
    }

    private void GiveFeedbackAndRating()
    {
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
            Feedback = new FeedbackData { Feedback = feedback, Rating = rating }
        };

        client.SendDataToServer(requestData);
        ShowMenuOptions();
    }

    /*private void ViewFeedbackAndRating()
    {
        Console.WriteLine("Requesting feedback and ratings from server...");

        CustomData requestData = new CustomData
        {
            Choice = "viewFeedbackAndRating"
        };

        try
        {
            CustomData responseData = client.SendDataToServer(requestData);
            if (responseData != null)
            {
                Console.WriteLine("Received feedback and ratings:");
                foreach (FeedbackItem item in responseData.FeedbackItems)
                {
                    Console.WriteLine($"Feedback: {item.Feedback}, Rating: {item.Rating}");
                }
            }
            else
            {
                Console.WriteLine("Failed to retrieve feedback and ratings from server.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }*/
}


