using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.ViewModels
{
    public class EditTaskViewModel : INotifyPropertyChanged
    {
        private readonly TodoApiService _apiService;

        public TodoTask Task { get; set; }

        public ICommand SaveCommand { get; }

        public EditTaskViewModel(TodoTask task, TodoApiService apiService)
        {
            Task = task;
            _apiService = apiService;
            SaveCommand = new Command(async () => await SaveTaskAsync());
        }

        private async Task SaveTaskAsync()
        {
            try
            {
                await _apiService.UpdateTaskAsync(Task); // Zamiast AddTaskAsync używamy UpdateTaskAsync
                await App.Current.MainPage.DisplayAlert("Success", "Task updated successfully", "OK");
                await App.Current.MainPage.Navigation.PopAsync(); // Powrót do poprzedniej strony
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Failed to update task: {ex.Message}", "OK");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
