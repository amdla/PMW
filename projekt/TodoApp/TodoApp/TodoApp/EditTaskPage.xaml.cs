using TodoApp.Models;
using TodoApp.Services;
using TodoApp.ViewModels;

namespace TodoApp;

public partial class EditTaskPage : ContentPage
{
    private readonly EditTaskViewModel _viewModel;

    public EditTaskPage(TodoTask task, TodoApiService apiService)
    {
        InitializeComponent();
        _viewModel = new EditTaskViewModel(task, apiService);
        BindingContext = _viewModel;
    }
}
