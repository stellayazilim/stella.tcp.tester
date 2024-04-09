using stella.socket.tester.Models;
namespace stella.socket.tester.Data;

public class Clients
{
    private static List<Client> _clients = new();


    public List<Client> GetClients()
    {
        return _clients;
    }

    public void AddClient(Client data)
    {
        _clients.Add(data);
    }
}