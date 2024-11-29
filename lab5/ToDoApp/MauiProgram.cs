using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;
using ToDoApp.Data;
using ToDoApp.Models;
using ToDoApp.Services;
using ToDoApp.ViewModels;
using ToDoApp.Views;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp
{
    public static class MauiProgram
    {
        public static IServiceProvider ServiceProvider { get; private set; }

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

            // Rejestracja DbContext z SQLite
            builder.Services.AddDbContext<ToDoDbContext>(options => { options.UseSqlite("Data Source=ToDoApp.db"); });

            // Rejestracja serwisów
            builder.Services.AddScoped<IToDoService, ToDoService>();

            // Rejestracja ViewModel-i
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddTransient<ToDoItemViewModel>();
            builder.Services.AddTransient<CategoryViewModel>();
            builder.Services.AddTransient<UserViewModel>();
            builder.Services.AddTransient<ToDoItemCategoryViewModel>();

            // Rejestracja widoków
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<ToDoItemPage>();
            builder.Services.AddTransient<CategoryPage>();
            builder.Services.AddTransient<UserPage>();

            // Inicjalizacja danych
            InitializeDatabase(builder.Services);

            var app = builder.Build();
            ServiceProvider = app.Services; // Przypisanie instancji ServiceProvider
            return app;
        }

        private static void InitializeDatabase(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();

            // Tworzenie bazy danych
            dbContext.Database.EnsureCreated();

            // Dodanie danych testowych, jeśli baza jest pusta
            if (!dbContext.Users.Any())
            {
                var user1 = new User { Name = "Jan Kowalski", Email = "jan.kowalski@example.com" };
                var user2 = new User { Name = "Anna Nowak", Email = "anna.nowak@example.com" };

                var category1 = new Category { Name = "Zakupy" };
                var category2 = new Category { Name = "Praca" };

                var task1 = new ToDoItem
                {
                    Title = "Kupić mleko",
                    Description = "Nie zapomnieć o chlebie",
                    IsCompleted = false,
                    User = user1
                };

                var task2 = new ToDoItem
                {
                    Title = "Zrobić raport",
                    Description = "Przygotować do poniedziałku",
                    IsCompleted = true,
                    User = user2
                };

                dbContext.Users.AddRange(user1, user2);
                dbContext.Categories.AddRange(category1, category2);
                dbContext.ToDoItems.AddRange(task1, task2);

                var taskCategory1 = new ToDoItemCategory { ToDoItem = task1, Category = category1 };
                var taskCategory2 = new ToDoItemCategory { ToDoItem = task2, Category = category2 };

                dbContext.ToDoItemCategories.AddRange(taskCategory1, taskCategory2);

                dbContext.SaveChanges();
            }
        }
    }
}