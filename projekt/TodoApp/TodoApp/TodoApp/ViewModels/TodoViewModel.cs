using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.ViewModels
{
    public class TodoViewModel : INotifyPropertyChanged
    {
        private readonly TodoApiService _todoApiService;

        public ObservableCollection<TodoTask> Tasks { get; set; } = new();

        public TodoViewModel(TodoApiService todoApiService)
        {
            _todoApiService = todoApiService;
            LoadTasks();
        }

        public async void LoadTasks()
        {
            var tasks = await _todoApiService.GetTasksAsync();
            foreach (var task in tasks)
            {
                Console.WriteLine($"Task: {task.Title}, Description: {task.Description}");
            }
            Tasks.Clear();
            foreach (var task in tasks)
            {
                Tasks.Add(task);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
