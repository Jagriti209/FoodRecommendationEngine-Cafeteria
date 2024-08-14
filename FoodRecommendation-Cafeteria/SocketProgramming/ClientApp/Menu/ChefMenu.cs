public class ChefMenu
{
    private ChefMenuOperations menuOperations;
    private  Logout logout;
    public ChefMenu(Client client)
    {
        menuOperations = new ChefMenuOperations(client);
        logout = new Logout(client);
    }
    public void DisplayMenu()
    {
        while (true)
        {
            ShowMenuOptions();
            string choice = GetUserChoice();

            if (choice == "7")
            {
                logout.LogoutUser();
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
        Console.WriteLine("4. View Rolled out items votes");
        Console.WriteLine("5. View Feedback");
        Console.WriteLine("6. View Menu Items to be Discarded");
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
                menuOperations.ViewRecommendation();
                break;
            case "3":
                menuOperations.CreateMenuForNextDay();
                break;
            case "4":
                menuOperations.ViewRolledOutMenu();
                break;
            case "5":
                menuOperations.ViewFeedback();
                break;
            case "6":
                menuOperations.ViewDiscardMenuItems();
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }
}
