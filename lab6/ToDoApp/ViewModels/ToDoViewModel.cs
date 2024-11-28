using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.ViewModels
{
    public partial class ToDoViewModel : ObservableObject
    {
        private readonly IToDoService _toDoService;

        public ObservableCollection<ToDoItem> Items { get; } = new();

        [ObservableProperty] private string title;

        [ObservableProperty] private string description;

        [ObservableProperty] private ToDoItem currentItem;

        public IAsyncRelayCommand LoadItemsCommand { get; }
        public IAsyncRelayCommand AddItemCommand { get; }
        public IAsyncRelayCommand<ToDoItem> DeleteItemCommand { get; }
        public IAsyncRelayCommand UpdateItemCommand { get; }
        public RelayCommand<ToDoItem> StartUpdateCommand { get; }

        public ToDoViewModel() // Domyślny konstruktor (dla XAML)
        {
            _toDoService = null!;
        }

        public ToDoViewModel(IToDoService toDoService)
        {
            _toDoService = toDoService;

            LoadItemsCommand = new AsyncRelayCommand(LoadItemsAsync);
            AddItemCommand = new AsyncRelayCommand(AddItemAsync);
            DeleteItemCommand = new AsyncRelayCommand<ToDoItem>(DeleteItemAsync);
            UpdateItemCommand = new AsyncRelayCommand(UpdateItemAsync);
            StartUpdateCommand = new RelayCommand<ToDoItem>(StartUpdate);

            Task.Run(async () => await LoadItemsAsync());
        }

        private async Task LoadItemsAsync()
        {
            if (_toDoService == null) return;

            var items = await _toDoService.GetItemsAsync();
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        private async Task AddItemAsync()
        {
            if (_toDoService == null) return;

            if (CurrentItem == null)
            {
                var newItem = new ToDoItem
                {
                    Title = Title,
                    Description = Description,
                    IsCompleted = false
                };

                await _toDoService.AddItemAsync(newItem);
            }
            else
            {
                CurrentItem.Title = Title;
                CurrentItem.Description = Description;

                await _toDoService.UpdateItemAsync(CurrentItem);
                CurrentItem = null;
            }

            await LoadItemsAsync();

            Title = string.Empty;
            Description = string.Empty;
        }

        private async Task DeleteItemAsync(ToDoItem item)
        {
            if (item == null || _toDoService == null) return;

            await _toDoService.DeleteItemAsync(item.Id);
            await LoadItemsAsync();
        }

        private async Task UpdateItemAsync()
        {
            if (CurrentItem == null || _toDoService == null) return;

            CurrentItem.Title = Title;
            CurrentItem.Description = Description;

            await _toDoService.UpdateItemAsync(CurrentItem);
            await LoadItemsAsync();

            Title = string.Empty;
            Description = string.Empty;
            CurrentItem = null;
        }

        private void StartUpdate(ToDoItem item)
        {
            if (item == null) return;

            CurrentItem = item;
            Title = item.Title;
            Description = item.Description;
        }
    }
}