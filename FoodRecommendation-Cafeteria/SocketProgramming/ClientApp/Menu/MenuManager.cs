public class MenuManager
{
    private readonly string _userRole;
    private readonly Client _client;

    public MenuManager(string userRole)
    {
        _userRole = userRole;
        _client = new Client(); // Initialize Client here or consider injecting it
    }

    public void DisplayMenu()
    {
        switch (_userRole.ToLower())
        {
            case "admin":
                new AdminMenu(_client).DisplayMenu();
                break;
            case "chef":
                new ChefMenu(_client).DisplayMenu();
                break;
            case "employee":
                new EmployeeMenu(_client).DisplayMenu();
                break;
            default:
                Console.WriteLine("Unknown role.");
                break;
        }
    }
}
