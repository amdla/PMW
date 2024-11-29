using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoApp.Views;

namespace ToDoApp.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    public IRelayCommand NavigateToToDoItemsCommand { get; }
    public IRelayCommand NavigateToCategoriesCommand { get; }
    public IRelayCommand NavigateToUsersCommand { get; }

    public MainPageViewModel()
    {
        NavigateToToDoItemsCommand = new RelayCommand(NavigateToToDoItems);
        NavigateToCategoriesCommand = new RelayCommand(NavigateToCategories);
        NavigateToUsersCommand = new RelayCommand(NavigateToUsers);
    }

    private async void NavigateToToDoItems()
    {
        var page = MauiProgram.ServiceProvider.GetService<ToDoItemPage>();
        if (page != null)
        {
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
    }

    private async void NavigateToCategories()
    {
        var page = MauiProgram.ServiceProvider.GetService<CategoryPage>();
        if (page != null)
        {
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
    }

    private async void NavigateToUsers()
    {
        var page = MauiProgram.ServiceProvider.GetService<UserPage>();
        if (page != null)
        {
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}