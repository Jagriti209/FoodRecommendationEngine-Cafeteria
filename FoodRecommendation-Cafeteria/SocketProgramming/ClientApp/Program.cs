class Program
{
    static void Main(string[] args)
    {
        try
        {
            var authenticationService = new AuthenticationService();
            var authenticationManager = new AuthenticationManager(authenticationService);
            authenticationManager.RunAuthentication();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
        }
    }
}
