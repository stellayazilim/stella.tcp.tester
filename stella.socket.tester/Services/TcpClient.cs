using System.IO;
using System.Net.Sockets;
using System.Text;

namespace stella.socket.tester.Services;

public class TcpService
{
    public  TcpClient _client { get; private set; }

    private readonly string _host;
    private readonly int _port;
    public TcpService(string host, int port)
    {
        _client = new TcpClient(host, port);
    
        _port = port;
        _host = host;

    }
     
    public async Task SendString(string data)
    {
        if (!_client.Connected) await _client.ConnectAsync(_host, _port);
        NetworkStream stream = _client.GetStream();
       
   
        var bytes = Encoding.UTF8.GetBytes(data);
        stream.Write(Encoding.UTF8.GetBytes(data), 0, bytes.Length);
    
        stream.Close();
        _client = new TcpClient(_host, _port);
    
    }


    public async void Connect()
    {
       if(!_client.Connected)  _client = new TcpClient(_host, _port);
    }
    public void Disconnect()
    {
        _client.Close();
        _client.Dispose();
    }
}