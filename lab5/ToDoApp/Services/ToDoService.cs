using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public interface IToDoService
    {
        // ToDoItem CRUD
        Task<List<ToDoItem>> GetItemsAsync();
        Task AddItemAsync(ToDoItem item);
        Task UpdateItemAsync(ToDoItem item);
        Task DeleteItemAsync(int id);

        // Category CRUD
        Task<List<Category>> GetCategoriesAsync();
        Task AddCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
        Task UpdateCategoryAsync(Category category);

        // ToDoItemCategory (Relationship)
        Task<List<ToDoItemCategory>> GetToDoItemCategoriesAsync();
        Task AddToDoItemCategoryAsync(ToDoItemCategory toDoItemCategory);
        Task DeleteToDoItemCategoryAsync(int id);

        // User CRUD
        Task<List<User>> GetUsersAsync();
        Task AddUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task UpdateUserAsync(User user);
    }

    public class ToDoService : IToDoService
    {
        private readonly ToDoDbContext _dbContext;

        public ToDoService(ToDoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ToDoItem CRUD
        public async Task<List<ToDoItem>> GetItemsAsync()
        {
            return await _dbContext.ToDoItems
                .Include(i => i.User)
                .Include(i => i.ToDoItemCategories)
                .ThenInclude(ic => ic.Category)
                .ToListAsync();
        }

        public async Task AddItemAsync(ToDoItem item)
        {
            _dbContext.ToDoItems.Add(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(ToDoItem item)
        {
            _dbContext.ToDoItems.Update(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(int id)
        {
            var item = await _dbContext.ToDoItems.FindAsync(id);
            if (item != null)
            {
                _dbContext.ToDoItems.Remove(item);
                await _dbContext.SaveChangesAsync();
            }
        }

        // Category CRUD
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _dbContext.Categories
                .Include(c => c.ToDoItemCategories)
                .ThenInclude(ic => ic.ToDoItem)
                .ToListAsync();
        }

        public async Task AddCategoryAsync(Category category)
        {
            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category != null)
            {
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();
            }
        }

        // User CRUD
        public async Task<List<User>> GetUsersAsync()
        {
            return await _dbContext.Users
                .Include(u => u.ToDoItems)
                .ToListAsync();
        }

        public async Task AddUserAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        // Relationship (ToDoItemCategory)
        public async Task<List<ToDoItemCategory>> GetToDoItemCategoriesAsync()
        {
            return await _dbContext.ToDoItemCategories
                .Include(ic => ic.ToDoItem)
                .Include(ic => ic.Category)
                .ToListAsync();
        }

        public async Task AddToDoItemCategoryAsync(ToDoItemCategory toDoItemCategory)
        {
            _dbContext.ToDoItemCategories.Add(toDoItemCategory);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteToDoItemCategoryAsync(int id)
        {
            var toDoItemCategory = await _dbContext.ToDoItemCategories.FindAsync(id);
            if (toDoItemCategory != null)
            {
                _dbContext.ToDoItemCategories.Remove(toDoItemCategory);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}