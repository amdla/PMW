using ToDoApp.Services;
using ToDoApp.Views;
using ToDoApp.ViewModels;
using Microsoft.Extensions.Logging;

namespace ToDoApp
{

    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                });

            // Rejestracja serwisu
            builder.Services.AddSingleton<IToDoService, ToDoService>();

            // Rejestracja ViewModelu
            builder.Services.AddSingleton<ToDoViewModel>();

            // Rejestracja widoku MainPage
            builder.Services.AddSingleton<UsersPage>();

            return builder.Build();
        }
    }
}

