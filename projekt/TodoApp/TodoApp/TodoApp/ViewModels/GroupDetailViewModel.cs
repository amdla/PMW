using System.Collections.ObjectModel;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.ViewModels
{
    public class GroupDetailViewModel
    {
        private readonly TodoApiService _apiService;
        private readonly Group _group;

        public ObservableCollection<TodoTask> Tasks { get; } = new();
        public TodoApiService ApiService => _apiService;

        public GroupDetailViewModel(Group group, TodoApiService apiService)
        {
            _group = group;
            _apiService = apiService;

            foreach (var task in group.Tasks)
            {
                Tasks.Add(task);
            }
        }

        public async Task AddTaskAsync(TodoTask newTask)
        {
            newTask.GroupId = _group.GroupId;
            newTask.GroupName = _group.Name;

            await _apiService.AddTaskAsync(newTask);
            Tasks.Add(newTask); // Immediately update the UI
        }

        public async Task RemoveTaskAsync(TodoTask task)
        {
            try
            {
                await _apiService.DeleteTaskAsync(task.TaskId);
                Tasks.Remove(task); // Usuń zadanie z lokalnej listy

                if (Tasks.Count == 0)
                {
                    bool confirm = await Application.Current.MainPage.DisplayAlert(
                        "Group is empty",
                        "This was the last task in the group. Do you want to delete the group?",
                        "Yes",
                        "No"
                    );

                    if (confirm)
                    {
                        await _apiService.DeleteGroupAsync(_group.GroupId);
                        // Powrót do listy grup
                        await Shell.Current.GoToAsync("..");             
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to delete task or group: {ex.Message}", "OK");
            }
        }
        public async Task RefreshTasksAsync()
        {
            var tasksFromApi = await _apiService.GetTasksAsync();
            Tasks.Clear();

            // Dodaj tylko zadania należące do tej grupy
            foreach (var task in tasksFromApi.Where(t => t.GroupId == _group.GroupId))
            {
                Tasks.Add(task);
            }
        }

    }
}
