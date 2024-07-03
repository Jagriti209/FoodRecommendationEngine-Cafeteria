public class AuthenticationManager
{
    private readonly AuthenticationService _authenticationService;

    public AuthenticationManager(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public void RunAuthentication()
    {
        Console.Write("Enter Name: ");
        string name = Console.ReadLine();
        Console.Write("Enter Password: ");
        string password = Console.ReadLine();

        var authResult = _authenticationService.Authenticate(name, password);

        if (authResult.Authenticated)
        {
            Console.WriteLine($"Authentication successful.\nWelcome {name}");
            MenuManager menuManager = new MenuManager(authResult.UserRole);
            menuManager.DisplayMenu();
        }
        else
        {
            Console.WriteLine("Authentication failed. Invalid username or password.");
        }
    }
}
