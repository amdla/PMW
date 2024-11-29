using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.ViewModels
{
    public partial class UserViewModel : ObservableObject
    {
        private readonly IToDoService _toDoService;

        public ObservableCollection<User> Users { get; } = new();

        [ObservableProperty] private string userName;
        [ObservableProperty] private string userEmail;
        [ObservableProperty] private User selectedUser;
        [ObservableProperty] private bool isEditing;

        public IAsyncRelayCommand LoadUsersCommand { get; }
        public IAsyncRelayCommand SaveUserCommand { get; }
        public IAsyncRelayCommand<User> DeleteUserCommand { get; }
        public IRelayCommand<User> EditUserCommand { get; }

        public UserViewModel(IToDoService toDoService)
        {
            _toDoService = toDoService;

            LoadUsersCommand = new AsyncRelayCommand(LoadUsersAsync);
            SaveUserCommand = new AsyncRelayCommand(SaveUserAsync);
            DeleteUserCommand = new AsyncRelayCommand<User>(DeleteUserAsync);
            EditUserCommand = new RelayCommand<User>(EditUser);
        }

        private async Task LoadUsersAsync()
        {
            var users = await _toDoService.GetUsersAsync();
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        private async Task SaveUserAsync()
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(UserEmail)) return;

            if (IsEditing && SelectedUser != null)
            {
                SelectedUser.Name = UserName;
                SelectedUser.Email = UserEmail;
                await _toDoService.UpdateUserAsync(SelectedUser);
            }
            else
            {
                var newUser = new User
                {
                    Name = UserName,
                    Email = UserEmail
                };
                await _toDoService.AddUserAsync(newUser);
            }

            await LoadUsersAsync();

            UserName = string.Empty;
            UserEmail = string.Empty;
            SelectedUser = null;
            IsEditing = false;
        }

        private void EditUser(User user)
        {
            if (user == null) return;

            SelectedUser = user;
            UserName = user.Name;
            UserEmail = user.Email;
            IsEditing = true;
        }

        private async Task DeleteUserAsync(User user)
        {
            if (user == null) return;

            await _toDoService.DeleteUserAsync(user.Id);
            await LoadUsersAsync();
        }
    }
}