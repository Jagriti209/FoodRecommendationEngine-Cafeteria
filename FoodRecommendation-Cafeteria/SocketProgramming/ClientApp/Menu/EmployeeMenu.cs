public class EmployeeMenu
{
    private EmployeeMenuOperations menuOperations;
    private  Logout logout;
    public EmployeeMenu(Client client)
    {
        menuOperations = new EmployeeMenuOperations(client);
        logout = new Logout(client);
    }
    public void DisplayMenu()
    {
        while (true)
        {
            ShowMenuOptions();
            string choice = GetUserChoice();

            if (choice == "8")
            {
                logout.LogoutUser();
                break;
            }

            ProcessChoice(choice);
        }
    }

    private void ShowMenuOptions()
    {
        Console.WriteLine("Employee actions:");
        Console.WriteLine("1. View Menu");
        Console.WriteLine("2. View Rolled out Menu");
        Console.WriteLine("3. Give Feedback & Rating");
        Console.WriteLine("4. View Feedback and Rating");
        Console.WriteLine("5. View Notifications");
        Console.WriteLine("6. Get Recommendation For me");
        Console.WriteLine("7. Update Profile");
        Console.WriteLine("8. Give feedback to dicarded item");
        Console.WriteLine("9. Logout");
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
                menuOperations.ViewRolledOutMenu();
                break;
            case "3":
                menuOperations.GiveFeedbackAndRating();
                break;
            case "4":
                menuOperations.ViewFeedbackAndRating();
                break;
            case "5":
                menuOperations.ViewNotifications();
                break;
            case "6":
                menuOperations.GiveRecommendationForMe();
                break;
            case "7":
                menuOperations.updateProfile() ;
                break;
            case "8":
                menuOperations.addFeedBackToDiscardedItem() ;
                break;
            case "9":
                Console.WriteLine("Exiting...");
                return;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }
}


