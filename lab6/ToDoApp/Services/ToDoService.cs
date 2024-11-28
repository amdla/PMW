using SQLite;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public interface IToDoService
    {
        Task<List<ToDoItem>> GetItemsAsync();
        Task AddItemAsync(ToDoItem item);
        Task UpdateItemAsync(ToDoItem item);
        Task DeleteItemAsync(int id);
    }

    public class ToDoService : IToDoService
    {
        private readonly SQLiteAsyncConnection _database;

        public ToDoService()
        {
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "ToDoDatabase.db");
            // location: \AppData\Local\Packages\com.companyname.todoapp_9zz4h110yvjzm\LocalState
            _database = new SQLiteAsyncConnection(databasePath);
            _database.CreateTableAsync<ToDoItem>().Wait();
        }

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
    }
}