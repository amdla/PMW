using ToDoApp.ViewModels;

namespace ToDoApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(ToDoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            Task.Run(async () => await viewModel.LoadItemsCommand.ExecuteAsync(null));
        }

    }
}
