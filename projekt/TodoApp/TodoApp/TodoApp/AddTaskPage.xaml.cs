using TodoApp.Models;
using TodoApp.ViewModels;
using TodoApp.Services;

namespace TodoApp;

public partial class AddTaskPage : ContentPage
{
    private readonly GroupDetailViewModel _viewModel;

    public AddTaskPage(GroupDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
    }

    private async void OnSaveTaskClicked(object sender, EventArgs e)
    {
        var newTask = new TodoTask
        {
            Title = TaskTitleEntry.Text,
            Description = TaskDescriptionEntry.Text,
            IsCompleted = false
        };

        await _viewModel.AddTaskAsync(newTask);

        // Powrót do poprzedniego widoku po dodaniu taska
        await Navigation.PopAsync();
    }
}
