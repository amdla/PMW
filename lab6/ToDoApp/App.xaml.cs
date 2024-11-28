using System.Diagnostics;
using ToDoApp.Views;

namespace ToDoApp
{
    public partial class App : Application
    {
        public App(MainPage mainPage)
        {
            InitializeComponent();

            // Global handler for unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Debug.WriteLine($"Unhandled exception: {e.ExceptionObject}");
            };

            // Set the main page for the app
            MainPage = mainPage;
        }
    }
}