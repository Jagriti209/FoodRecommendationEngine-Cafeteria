using System.Net.Sockets;
public class Logout
{
    private Client client;
      public Logout(Client client)
    {
        this.client = client;
        
    }
    public void LogoutUser()
    {
        CustomData sendData = new CustomData
        {
            Choice = "logout",
            UserData = new UserData { UserID = AuthenticationService.UserId }
        };
        client.SendDataToServer(sendData);
    }
}
    

