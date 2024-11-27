using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class ToDoService : IToDoService
    {
        private readonly string _filePath;

        public ToDoService()
        {
            _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "todoitems.json");
        }

        public async Task<List<ToDoItem>> GetItemsAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<ToDoItem>();
            }

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<ToDoItem>>(json) ?? new List<ToDoItem>();
        }

        public async Task AddItemAsync(ToDoItem item)
        {
            var items = await GetItemsAsync();
            item.Id = items.Any() ? items.Max(i => i.Id) + 1 : 1;
            items.Add(item);
            await SaveItemsAsync(items);
        }

        public async Task UpdateItemAsync(ToDoItem item)
        {
            var items = await GetItemsAsync();
            var existingItem = items.FirstOrDefault(i => i.Id == item.Id);
            if (existingItem != null)
            {
                existingItem.Title = item.Title;
                existingItem.Description = item.Description;
                existingItem.IsCompleted = item.IsCompleted;
            }
            await SaveItemsAsync(items);
        }

        public async Task DeleteItemAsync(int id)
        {
            var items = await GetItemsAsync();
            var item = items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                items.Remove(item);
            }
            await SaveItemsAsync(items);
        }

        private async Task SaveItemsAsync(List<ToDoItem> items)
        {
            var json = JsonSerializer.Serialize(items);
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
