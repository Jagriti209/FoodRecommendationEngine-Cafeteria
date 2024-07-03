public class ChefMenu
{
    private ChefMenuOperations menuOperations;

    public ChefMenu(Client client)
    {
        menuOperations = new ChefMenuOperations(client);
    }
    public void DisplayMenu()
    {
        while (true)
        {
            ShowMenuOptions();
            string choice = GetUserChoice();

            if (choice == "6")
            {
                Console.WriteLine("Logging out...");
                break;
            }

            ProcessChoice(choice);
        }
    }

    private void ShowMenuOptions()
    {
        Console.WriteLine("Chef actions:");
        Console.WriteLine("1. View Menu");
        Console.WriteLine("2. View Recommendation");
        Console.WriteLine("3. Create Menu for Next Day");
        Console.WriteLine("4. View Feedback");
        Console.WriteLine("5. View Discard Menu Items");
        Console.WriteLine("6. Logout");
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
                menuOperations.ViewRecommendation();
                break;
            case "3":
                menuOperations.CreateMenuForNextDay();
                break;
            case "4":
                menuOperations.ViewFeedback();
                break;
            case "5":
                menuOperations.ViewDiscardMenuItems();
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }
}
