using SQLite;
using ToDoApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ToDoApp.Services
{
    public interface IToDoService
    {
        // CRUD dla ToDoItem
        Task<List<ToDoItem>> GetItemsAsync();
        Task AddItemAsync(ToDoItem item);
        Task UpdateItemAsync(ToDoItem item);
        Task DeleteItemAsync(int id);

        // CRUD dla Category
        Task<List<Category>> GetCategoriesAsync();
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);

        // CRUD dla ToDoItemCategory (wiele do wielu)
        Task<List<Category>> GetCategoriesForToDoItemAsync(int toDoItemId);
        Task AssignCategoriesToToDoItemAsync(int toDoItemId, List<int> categoryIds);
        Task RemoveCategoryFromToDoItemAsync(int toDoItemId, int categoryId);

        // CRUD dla User
        Task<User> GetUserAsync(int id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }

    public class ToDoService : IToDoService
    {
        private readonly SQLiteAsyncConnection _database;

        public ToDoService()
        {
            var databasePath = Path.Combine("ToDoDatabase.db"); // Zmień na rzeczywistą ścieżkę, jeśli to konieczne
            _database = new SQLiteAsyncConnection(databasePath);
            _database.CreateTableAsync<ToDoItem>().Wait();
            _database.CreateTableAsync<Category>().Wait();
            _database.CreateTableAsync<ToDoItemCategory>().Wait();
            _database.CreateTableAsync<User>().Wait();
        }

        // CRUD dla ToDoItem
        public Task<List<ToDoItem>> GetItemsAsync()
        {
            return _database.Table<ToDoItem>().ToListAsync();
        }

        public Task AddItemAsync(ToDoItem item)
        {
            return _database.InsertAsync(item);
        }

        public Task UpdateItemAsync(ToDoItem item)
        {
            return _database.UpdateAsync(item);
        }

        public Task DeleteItemAsync(int id)
        {
            return _database.DeleteAsync<ToDoItem>(id);
        }

        // CRUD dla Category
        public Task<List<Category>> GetCategoriesAsync()
        {
            return _database.Table<Category>().ToListAsync();
        }

        public Task AddCategoryAsync(Category category)
        {
            return _database.InsertAsync(category);
        }

        public Task UpdateCategoryAsync(Category category)
        {
            return _database.UpdateAsync(category);
        }

        public Task DeleteCategoryAsync(int id)
        {
            return _database.DeleteAsync<Category>(id);
        }

        // CRUD dla ToDoItemCategory (relacja wiele do wielu)
        public Task<List<Category>> GetCategoriesForToDoItemAsync(int toDoItemId)
        {
            return _database.QueryAsync<Category>(
                "SELECT Category.* FROM Category " +
                "JOIN ToDoItemCategory ON Category.Id = ToDoItemCategory.CategoryId " +
                "WHERE ToDoItemCategory.ToDoItemId = ?", toDoItemId);
        }

        public async Task AssignCategoriesToToDoItemAsync(int toDoItemId, List<int> categoryIds)
        {
            // Usuwamy istniejące powiązania
            await _database.ExecuteAsync("DELETE FROM ToDoItemCategory WHERE ToDoItemId = ?", toDoItemId);

            // Dodajemy nowe powiązania
            foreach (var categoryId in categoryIds)
            {
                var toDoItemCategory = new ToDoItemCategory
                {
                    ToDoItemId = toDoItemId,
                    CategoryId = categoryId
                };
                await _database.InsertAsync(toDoItemCategory);
            }
        }

        public async Task RemoveCategoryFromToDoItemAsync(int toDoItemId, int categoryId)
        {
            await _database.ExecuteAsync(
                "DELETE FROM ToDoItemCategory WHERE ToDoItemId = ? AND CategoryId = ?",
                toDoItemId, categoryId);
        }

        // CRUD dla User
        public Task<User> GetUserAsync(int id)
        {
            return _database.Table<User>().FirstOrDefaultAsync(u => u.Id == id);
        }

        public Task AddUserAsync(User user)
        {
            return _database.InsertAsync(user);
        }

        public Task UpdateUserAsync(User user)
        {
            return _database.UpdateAsync(user);
        }

        public Task DeleteUserAsync(int id)
        {
            return _database.DeleteAsync<User>(id);
        }
    }
}