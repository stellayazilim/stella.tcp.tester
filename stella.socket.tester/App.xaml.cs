
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using stella.socket.tester.Data;
using stella.socket.tester.Helpers;
using stella.socket.tester.Services;
using stella.socket.tester.ViewModels;

namespace stella.socket.tester;


public partial class App : Application
{

    private readonly IHost _app;

    public App()
    {
        _app = Host.CreateDefaultBuilder()
            .ConfigureServices(
                services =>
                {
                    services.AddScoped<IpValidator>();
                    services.AddSingleton<Clients>();
                    services.AddSingleton<MainWindowViewModel>();
                    services.AddSingleton<MainWindow>();

                }).Build();
    }

    protected override void OnStartup(StartupEventArgs e)
   {
       _app.Start();
       var window = _app.Services.GetRequiredService<MainWindow>();
       window!.Show();
       base.OnStartup(e);
   }
}