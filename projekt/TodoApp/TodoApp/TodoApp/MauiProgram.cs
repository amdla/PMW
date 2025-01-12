using TodoApp.ViewModels;
using TodoApp.Services;
using TodoApp;
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
            });

        // Rejestracja serwisów i ViewModel
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton<TodoApiService>();
        builder.Services.AddSingleton<TodoViewModel>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<GroupDetailPage>();
        builder.Services.AddSingleton<GroupViewModel>();

        return builder.Build();
    }
}
