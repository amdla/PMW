using TodoApp.Models;
using TodoApp.ViewModels;
using TodoApp.Services;

namespace TodoApp;

public partial class MainPage : ContentPage
{
    private readonly TodoApiService _apiService;
    private readonly GroupViewModel _viewModel;

    public MainPage(GroupViewModel groupViewModel, TodoApiService apiService)
    {
        InitializeComponent();
        _viewModel = groupViewModel;

        BindingContext = groupViewModel;

        _apiService = apiService; // Przypisanie instancji serwisu
    }

    private async void OnGroupSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Group selectedGroup)
        {
            // Przekazanie instancji TodoApiService do GroupDetailPage
            await Navigation.PushAsync(new GroupDetailPage(selectedGroup, _apiService));
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private async void OnAddGroupClicked(object sender, EventArgs e)
    {
        if (BindingContext is GroupViewModel groupViewModel)
        {
            await Navigation.PushAsync(new AddGroupPage(groupViewModel));
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.RefreshGroupsAsync();
    }

    private async void OnGroupCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.BindingContext is Group group)
        {
            if (group.IsChecked) // Checkbox zaznaczony
            {
                if (group.Tasks.Any())
                {
                    await DisplayAlert("Cannot Delete Group",
                        "This group contains tasks. Remove all tasks before deleting the group.",
                        "OK");
                    group.IsChecked = false; // Odznacz checkbox
                    return;
                }

                bool confirm = await DisplayAlert("Confirm",
                    "Are you sure you want to delete this group?",
                    "Yes", "No");

                if (confirm)
                {
                    try
                    {
                        await _apiService.DeleteGroupAsync(group.GroupId);
                        _viewModel.Groups.Remove(group); // Usuń z lokalnej listy
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"Failed to delete group: {ex.Message}", "OK");
                    }
                }
                else
                {
                    group.IsChecked = false; // Odznacz jeśli użytkownik anulował
                }
            }
        }
    }


    private async void OnGroupTapped(object sender, EventArgs e)
    {
        if (sender is Label label && label.BindingContext is Group selectedGroup)
        {
            await Navigation.PushAsync(new GroupDetailPage(selectedGroup, _apiService));
        }
    }


}
