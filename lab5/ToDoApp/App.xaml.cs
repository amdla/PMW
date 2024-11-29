using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Views;

namespace ToDoApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Ustawienie MainPage owiniętej w NavigationPage
        MainPage = new NavigationPage(MauiProgram.ServiceProvider.GetService<MainPage>());
    }
}