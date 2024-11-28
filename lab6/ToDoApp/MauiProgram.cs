using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Services;
using ToDoApp.ViewModels;
using ToDoApp.Views;

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

            // Rejestracja serwisów i widoków
            builder.Services.AddSingleton<IToDoService, ToDoService>();
            builder.Services.AddSingleton<ToDoViewModel>();
            builder.Services.AddSingleton<MainPage>();

            return builder.Build();
        }
    }
}