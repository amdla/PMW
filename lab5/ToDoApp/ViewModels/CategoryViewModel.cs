using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.ViewModels
{
    public partial class CategoryViewModel : ObservableObject
    {
        private readonly IToDoService _toDoService;

        public ObservableCollection<Category> Categories { get; } = new();

        [ObservableProperty] private string categoryName;
        [ObservableProperty] private Category selectedCategory;
        [ObservableProperty] private bool isEditing;

        public IAsyncRelayCommand LoadCategoriesCommand { get; }
        public IAsyncRelayCommand SaveCategoryCommand { get; }
        public IAsyncRelayCommand<Category> DeleteCategoryCommand { get; }
        public IRelayCommand<Category> EditCategoryCommand { get; }

        public CategoryViewModel(IToDoService toDoService)
        {
            _toDoService = toDoService;

            LoadCategoriesCommand = new AsyncRelayCommand(LoadCategoriesAsync);
            SaveCategoryCommand = new AsyncRelayCommand(SaveCategoryAsync);
            DeleteCategoryCommand = new AsyncRelayCommand<Category>(DeleteCategoryAsync);
            EditCategoryCommand = new RelayCommand<Category>(EditCategory);
        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await _toDoService.GetCategoriesAsync();
            Categories.Clear();
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
        }

        private async Task SaveCategoryAsync()
        {
            if (string.IsNullOrWhiteSpace(CategoryName)) return;

            if (IsEditing && SelectedCategory != null)
            {
                SelectedCategory.Name = CategoryName;
                await _toDoService.UpdateCategoryAsync(SelectedCategory);
            }
            else
            {
                var newCategory = new Category
                {
                    Name = CategoryName
                };
                await _toDoService.AddCategoryAsync(newCategory);
            }

            await LoadCategoriesAsync();

            CategoryName = string.Empty;
            SelectedCategory = null;
            IsEditing = false;
        }

        private void EditCategory(Category category)
        {
            if (category == null) return;

            SelectedCategory = category;
            CategoryName = category.Name;
            IsEditing = true;
        }

        private async Task DeleteCategoryAsync(Category category)
        {
            if (category == null) return;

            await _toDoService.DeleteCategoryAsync(category.Id);
            await LoadCategoriesAsync();
        }
    }
}