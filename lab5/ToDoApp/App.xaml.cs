using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Views;

namespace ToDoApp
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            MainPage = serviceProvider.GetRequiredService<UsersPage>();
        }

    }
}
