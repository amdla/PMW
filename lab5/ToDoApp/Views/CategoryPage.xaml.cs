using ToDoApp.ViewModels;

namespace ToDoApp.Views
{
    public partial class CategoryPage : ContentPage
    {
        public CategoryPage(CategoryViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as CategoryViewModel)?.LoadCategoriesCommand.ExecuteAsync(null);
        }
    }
}