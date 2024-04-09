using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using stella.socket.tester.Helpers;
using stella.socket.tester.Models;
using stella.socket.tester.Services;

namespace stella.socket.tester.ViewModels;


public partial class MainWindowViewModel: ObservableObject
{


    private readonly IpValidator _ipValidator;
    public MainWindowViewModel(
        IpValidator ipValidator)
    {
        _ipValidator = ipValidator;
        
        Clients.Add(CurrentClient);
    }
    public List<ListenType> ListenTypes { get; } =
    [
        ListenType.Client,
        ListenType.Server
    ];

    public static Guid NewConnection = new Guid();
    
    
    [NotifyCanExecuteChangedFor(nameof(DisconnectCommandCommand))]
    [NotifyCanExecuteChangedFor(nameof(ConnectCommandCommand))]
    [ObservableProperty]
    private Client _currentClient = new Client
    {
        Id    = NewConnection,
        Addr= "New connection",
    };
    

    
    [NotifyCanExecuteChangedFor(nameof(ConnectCommandCommand))]
    [ObservableProperty] 
    private string _host = string.Empty;
    
    [NotifyCanExecuteChangedFor(nameof(ConnectCommandCommand))]
    [ObservableProperty] 
    private string _port = string.Empty;


    [ObservableProperty] 
    private ObservableCollection<Client> _clients = new();

    [ObservableProperty] 
    private ObservableCollection<Event> _events = new();

    [ObservableProperty]
    private ListenType _listenType = ListenType.Client;

    [ObservableProperty] 
    private string _data = string.Empty;

    [RelayCommand(CanExecute = nameof(CanConnect))]
    private async Task ConnectCommand()
    {
        
        if (CurrentClient.Id != NewConnection)
        {
            CurrentClient.IsConnected = true;
            CurrentClient.TcpClient.Connect();
            SetProperty(ref _currentClient, CurrentClient);
            return;
        } 
        
        var service = new TcpService(Host, int.Parse(Port));
        var client = new Client()
        {
            Addr = Host,
            Id = Guid.NewGuid(),
            Port = Port,
            IsConnected = true,
            TcpClient =  service
        };
        CurrentClient = client;
        
        Clients.Add(client);
        SetProperty(ref _clients, Clients);
  
    }
    

    [RelayCommand]
    private async void Send()
    {
         await CurrentClient.TcpClient.SendString(Data);
        
    }

    [RelayCommand(CanExecute = nameof(CanDisconnect))]
    private void DisconnectCommand()
    {
        CurrentClient.TcpClient.Disconnect();
        
        var t = CurrentClient;

        t.IsConnected = false;
        Clients.Insert(Clients.IndexOf(CurrentClient),t );
        CurrentClient.IsConnected = false;

        Clients.Remove(CurrentClient);
       
        
        SetProperty(ref _currentClient, CurrentClient);
        SetProperty(ref _clients, Clients);
    }


    [RelayCommand]
    private void SelectedItemChanged(object _client)
    {

        if (_client is not Client)
        {
            SetProperty(ref _currentClient, CurrentClient);
            
            return;
        }
           
        Client client = _client as Client;
        
            
        Host = NewConnection.ToString() == client.Id.ToString() ? "" : client.Addr;
        Port = NewConnection.ToString() == client.Id.ToString() ? "" : client.Port;
        CurrentClient = client;
        SetProperty(ref _host, Host);
        SetProperty(ref _port, Port);
        SetProperty(ref _currentClient, CurrentClient);

    }

    private bool CanConnect()
    {
        return _ipValidator.ValidateIPv4(Host) && !string.IsNullOrEmpty(Port) && !CurrentClient.IsConnected;
        
        
    }

    private bool CanDisconnect()
    {
        return CurrentClient.IsConnected;
    }
}