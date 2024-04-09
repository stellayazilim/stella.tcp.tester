using System.Net.Sockets;
using stella.socket.tester.Services;

namespace stella.socket.tester.Models;



public class Client
{
    
    public string Addr { get; set; }
    
    public string Port { get; set; }
    
    public Guid Id { get; set; }
    
    public TcpService TcpClient { get; set; }
    
    public bool IsConnected { get; set;  }
}