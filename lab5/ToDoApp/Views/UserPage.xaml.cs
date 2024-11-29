using ToDoApp.ViewModels;

namespace ToDoApp.Views
{
    public partial class UserPage : ContentPage
    {
        public UserPage(UserViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as UserViewModel)?.LoadUsersCommand.ExecuteAsync(null);
        }
    }
}