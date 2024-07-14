public class EmployeeMenu
{
    private EmployeeMenuOperations menuOperations;

    public EmployeeMenu(Client client)
    {
        menuOperations = new EmployeeMenuOperations(client);
    }
    public void DisplayMenu()
    {
        while (true)
        {
            ShowMenuOptions();
            string choice = GetUserChoice();

            if (choice == "7")
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
        Console.WriteLine("4. View Notifications");
        Console.WriteLine("5. Get Recommendation For me");
        Console.WriteLine("6. Update Profile");
        Console.WriteLine("7. Logout");
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
                menuOperations.ViewMenu();
                break;
            case "2":
                menuOperations.GiveFeedbackAndRating();
                break;
            case "3":
                menuOperations.ViewFeedbackAndRating();
                break;
            case "4":
                menuOperations.ViewNotifications();
                break;
            case "5":
                menuOperations.GiveRecommendationForMe();
                break;
            case "6":
                menuOperations.updateProfile() ;
                break;
            case "7":
                Console.WriteLine("Exiting...");
                return;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }
}


