using TodoApp.Models;
using TodoApp.Services;
using TodoApp.ViewModels;

namespace TodoApp;

public partial class AddGroupPage : ContentPage
{
    private readonly TodoApiService _apiService;
    private readonly GroupViewModel _groupViewModel;

    public AddGroupPage(GroupViewModel groupViewModel)
    {
        InitializeComponent();
        _apiService = new TodoApiService(new HttpClient()); // Mo¿esz zmieniæ na DI, jeœli potrzebne
        _groupViewModel = groupViewModel;
    }


    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        string groupName = GroupNameEntry.Text;

        if (string.IsNullOrWhiteSpace(groupName))
        {
            await DisplayAlert("B³¹d", "Nazwa grupy nie mo¿e byæ pusta.", "OK");
            return;
        }

        var newGroup = new Group { Name = groupName };
        await _apiService.AddGroupAsync(newGroup);

        await _groupViewModel.RefreshGroupsAsync(); // Odœwie¿enie danych w MainPage

        await DisplayAlert("Sukces", "Grupa zosta³a dodana.", "OK");
        await Navigation.PopAsync(); // Powrót do poprzedniej strony
    }
}
