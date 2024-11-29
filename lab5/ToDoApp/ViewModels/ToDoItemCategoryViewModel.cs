using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.ViewModels
{
    public partial class ToDoItemCategoryViewModel : ObservableObject
    {
        private readonly IToDoService _toDoService;

        public ObservableCollection<ToDoItemCategory> ToDoItemCategories { get; } = new();
        public ObservableCollection<ToDoItem> ToDoItems { get; } = new();
        public ObservableCollection<Category> Categories { get; } = new();

        [ObservableProperty] private ToDoItem selectedToDoItem;
        [ObservableProperty] private Category selectedCategory;

        public IAsyncRelayCommand LoadToDoItemCategoriesCommand { get; }
        public IAsyncRelayCommand AddToDoItemCategoryCommand { get; }
        public IAsyncRelayCommand<ToDoItemCategory> DeleteToDoItemCategoryCommand { get; }

        public ToDoItemCategoryViewModel(IToDoService toDoService)
        {
            _toDoService = toDoService;

            LoadToDoItemCategoriesCommand = new AsyncRelayCommand(LoadToDoItemCategoriesAsync);
            AddToDoItemCategoryCommand = new AsyncRelayCommand(AddToDoItemCategoryAsync);
            DeleteToDoItemCategoryCommand = new AsyncRelayCommand<ToDoItemCategory>(DeleteToDoItemCategoryAsync);
        }

        private async Task LoadToDoItemCategoriesAsync()
        {
            var toDoItemCategories = await _toDoService.GetToDoItemCategoriesAsync();
            var items = await _toDoService.GetItemsAsync();
            var categories = await _toDoService.GetCategoriesAsync();

            ToDoItemCategories.Clear();
            ToDoItems.Clear();
            Categories.Clear();

            foreach (var item in items)
                ToDoItems.Add(item);

            foreach (var category in categories)
                Categories.Add(category);

            foreach (var itemCategory in toDoItemCategories)
                ToDoItemCategories.Add(itemCategory);
        }

        private async Task AddToDoItemCategoryAsync()
        {
            if (SelectedToDoItem == null || SelectedCategory == null) return;

            var toDoItemCategory = new ToDoItemCategory
            {
                ToDoItemId = SelectedToDoItem.Id,
                CategoryId = SelectedCategory.Id
            };

            await _toDoService.AddToDoItemCategoryAsync(toDoItemCategory);
            await LoadToDoItemCategoriesAsync();
        }

        private async Task DeleteToDoItemCategoryAsync(ToDoItemCategory toDoItemCategory)
        {
            if (toDoItemCategory == null) return;

            await _toDoService.DeleteToDoItemCategoryAsync(toDoItemCategory.Id);
            await LoadToDoItemCategoriesAsync();
        }
    }
}