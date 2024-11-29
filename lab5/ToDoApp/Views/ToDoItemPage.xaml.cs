using ToDoApp.Services;
using ToDoApp.ViewModels;

namespace ToDoApp.Views
{
    public partial class ToDoItemPage : ContentPage
    {
        public ToDoItemPage(ToDoItemViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as ToDoItemViewModel)?.LoadItemsCommand.ExecuteAsync(null);
        }
    }
}