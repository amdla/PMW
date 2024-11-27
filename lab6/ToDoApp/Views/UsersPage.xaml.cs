using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Services;
using ToDoApp.ViewModels;

namespace ToDoApp.Views
{
    public partial class UsersPage : ContentPage
    {
        public UsersPage(ToDoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

            // Ładowanie elementów po inicjalizacji
            Task.Run(async () => await viewModel.LoadItemsCommand.ExecuteAsync(null));
        }
    }
}
