class Program
{
    static void Main(string[] args)
    {
        var authenticationService = new AuthenticationService();
        var client = new Client();

        try
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            var authResult = authenticationService.Authenticate(name, password);

            if (authResult.Authenticated)
            {
                Console.WriteLine($"Authentication successful.\nWelcome {name}");

                switch (authResult.UserRole.ToLower())
                {
                    case "admin":
                        new AdminMenu(client).DisplayMenu();
                        break;
                    case "chef":
                        new ChefMenu(client).DisplayMenu();
                        break;
                    case "employee":
                        new EmployeeMenu(client).DisplayMenu();
                        break;
                    default:
                        Console.WriteLine("Unknown role.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Authentication failed. Invalid username or password.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
        }
    }
}
