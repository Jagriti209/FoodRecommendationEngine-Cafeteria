using System.Net.Sockets;

public static class RoleHandler
{
    public static void RoleBasedAction(NetworkStream stream, string userRole, CustomData requestData)
    {
        switch (userRole.ToLower())
        {
            case "admin":
                AdminRequestHandler.ProcessAdminAction(stream, requestData);
                break;
            case "chef":
                ChefRequestHandler.ProcessChefAction(stream, requestData);
                break;
            case "employee":
                EmployeeRequestHandler.ProcessEmployeeAction(stream, requestData);
                break;
            default:
                Console.WriteLine("Invalid role type.");
                break;
        }
    }
}