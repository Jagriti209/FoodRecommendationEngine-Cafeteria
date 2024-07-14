using System.Globalization;

public class AdminMenu
{
    private AdminMenuOperations menuOperations;
    AuthenticationService authenticationService = new AuthenticationService();
    
    public AdminMenu(Client client)
    {
        menuOperations = new AdminMenuOperations(client);
    }

    public void DisplayMenu()
    {
        Console.WriteLine("Admin actions:");
        Console.WriteLine("1. Add Menu Item");
        Console.WriteLine("2. Update Menu Item");
        Console.WriteLine("3. Delete Menu Item");
        Console.WriteLine("4. View Menu Items");
        Console.WriteLine("5. View Feedback");
        Console.WriteLine("6. View Discard Menu Items");
        Console.WriteLine("7. Logout");

        while (true)
        {
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine().Trim().ToLower();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Fill details to add menu items.");
                    menuOperations.AddMenuItem();
                    break;
                case "2":
                    Console.WriteLine("Fill details to update menu item.");
                    menuOperations.UpdateMenuItem();
                    break;
                case "3":
                    Console.WriteLine("Fill details to delete menu item.");
                    menuOperations.DeleteMenuItem();
                    break;
                case "4":
                    Console.WriteLine("Viewing menu items...");
                    menuOperations.ViewMenuItems();
                    break;
                case "5":
                    menuOperations.ViewFeedback();
                    break;
                case "6":
                    menuOperations.ViewDiscardMenuItems();
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
}
