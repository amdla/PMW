using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.ViewModels
{
    public class GroupViewModel : INotifyPropertyChanged
    {
        private readonly TodoApiService _apiService;

        public ObservableCollection<Group> Groups { get; } = new();

        public GroupViewModel(TodoApiService apiService)
        {
            _apiService = apiService;
            LoadGroups();
        }

        private async void LoadGroups()
        {
            var groups = await _apiService.GetGroupsAsync();
            Groups.Clear();
            foreach (var group in groups)
            {
                Groups.Add(group);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task RefreshGroupsAsync()
        {
            var groups = await _apiService.GetGroupsAsync();
            Groups.Clear();
            foreach (var group in groups)
            {
                Groups.Add(group);
            }
        }
    }
}
