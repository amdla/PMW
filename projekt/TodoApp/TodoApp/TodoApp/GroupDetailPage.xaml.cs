using TodoApp.Models;
using TodoApp.ViewModels;
using TodoApp.Services;

namespace TodoApp;

public partial class GroupDetailPage : ContentPage
{
    private readonly GroupDetailViewModel _viewModel;

    public GroupDetailPage(Group group, TodoApiService apiService)
    {
        InitializeComponent();

        _viewModel = new GroupDetailViewModel(group, apiService);
        BindingContext = _viewModel;

        Title = group.Name; // Ustaw tytu³ na nazwê grupy


 

    }

    private bool _isEditMode = false;

    private void ToggleEditMode()
    {
        _isEditMode = !_isEditMode;

        if (_isEditMode)
        {
            TaskCollectionView.SelectionMode = SelectionMode.Single;
        }
        else
        {
            TaskCollectionView.SelectionMode = SelectionMode.None;
            TaskCollectionView.SelectedItem = null; // Reset zaznaczenia
        }
    }


    private async void OnTaskSelected(object sender, SelectionChangedEventArgs e)
    {
        if (_isEditMode && e.CurrentSelection.FirstOrDefault() is TodoTask selectedTask)
        {
            // PrzejdŸ do widoku edycji
            await Navigation.PushAsync(new EditTaskPage(selectedTask, _viewModel.ApiService));

            // Zresetuj selekcjê po przejœciu
            TaskCollectionView.SelectedItem = null;
        }
    }
    private void OnEditTaskButtonClicked(object sender, EventArgs e)
    {
        _isEditMode = !_isEditMode; // Prze³¹cz tryb edycji

        // Zmieñ tekst przycisku Edit Task w zale¿noœci od trybu
        EditTaskToolbarItem.Text = _isEditMode ? "Done Editing" : "Edit Task";

        // Zmieñ tryb wyboru w CollectionView
        TaskCollectionView.SelectionMode = _isEditMode ? SelectionMode.Single : SelectionMode.None;

        // Resetuj zaznaczenie, jeœli wychodzisz z trybu edycji
        if (!_isEditMode)
        {
            TaskCollectionView.SelectedItem = null;
        }
    }




    private async void OnAddTaskClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddTaskPage(_viewModel));
    }

    private async void OnTaskCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.BindingContext is TodoTask task)
        {
            // Potwierdzenie przed usuniêciem (opcjonalne)
            bool confirm = await DisplayAlert("Confirm", "Do you want to remove this task?", "Yes", "No");
            if (confirm)
            {
                await _viewModel.RemoveTaskAsync(task);
            }
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.RefreshTasksAsync(); // Odœwie¿enie listy zadañ
    }



}
