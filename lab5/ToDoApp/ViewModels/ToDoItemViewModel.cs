using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.ViewModels
{
    public partial class ToDoItemViewModel : ObservableObject
    {
        private readonly IToDoService _toDoService;

        public ObservableCollection<ToDoItem> ToDoItems { get; } = new();

        [ObservableProperty] private string title;
        [ObservableProperty] private string description;
        [ObservableProperty] private ToDoItem selectedToDoItem;
        [ObservableProperty] private bool isEditing; // Flaga trybu edycji

        public IAsyncRelayCommand LoadItemsCommand { get; }
        public IAsyncRelayCommand SaveItemCommand { get; }
        public IAsyncRelayCommand<ToDoItem> DeleteItemCommand { get; }
        public IRelayCommand<ToDoItem> EditItemCommand { get; }

        public ToDoItemViewModel(IToDoService toDoService)
        {
            _toDoService = toDoService;

            LoadItemsCommand = new AsyncRelayCommand(LoadItemsAsync);
            SaveItemCommand = new AsyncRelayCommand(SaveItemAsync);
            DeleteItemCommand = new AsyncRelayCommand<ToDoItem>(DeleteItemAsync);
            EditItemCommand = new RelayCommand<ToDoItem>(EditItem);
        }

        private async Task LoadItemsAsync()
        {
            var items = await _toDoService.GetItemsAsync();
            ToDoItems.Clear();
            foreach (var item in items)
            {
                ToDoItems.Add(item);
            }
        }

        private async Task SaveItemAsync()
        {
            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Description)) return;

            if (IsEditing && SelectedToDoItem != null)
            {
                // Aktualizacja istniejącego zadania
                SelectedToDoItem.Title = Title;
                SelectedToDoItem.Description = Description;
                await _toDoService.UpdateItemAsync(SelectedToDoItem);
            }
            else
            {
                // Dodawanie nowego zadania
                var newItem = new ToDoItem
                {
                    Title = Title,
                    Description = Description
                };
                await _toDoService.AddItemAsync(newItem);
            }

            await LoadItemsAsync();

            // Resetowanie formularza
            Title = string.Empty;
            Description = string.Empty;
            SelectedToDoItem = null;
            IsEditing = false;
        }

        private void EditItem(ToDoItem item)
        {
            if (item == null) return;

            // Wypełnianie formularza danymi do edycji
            SelectedToDoItem = item;
            Title = item.Title;
            Description = item.Description;
            IsEditing = true;
        }

        private async Task DeleteItemAsync(ToDoItem item)
        {
            if (item == null) return;

            await _toDoService.DeleteItemAsync(item.Id);
            await LoadItemsAsync();
        }
    }
}