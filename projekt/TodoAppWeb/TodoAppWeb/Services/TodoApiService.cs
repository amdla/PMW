using TodoAppWeb.Models;

namespace TodoAppWeb.Services
{
    public class TodoApiService
    {
        private readonly HttpClient _httpClient;

        public TodoApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private const string BaseApiUrl = "http://localhost:5279/api"; //10.0.2.2 dla androida i localhost dla windowsa


        public async Task<List<TodoTask>> GetTasksAsync()
        {
            string tasksEndpoint = $"{BaseApiUrl}/Tasks";
            return await _httpClient.GetFromJsonAsync<List<TodoTask>>(tasksEndpoint);
        }

        public async Task<List<Group>> GetGroupsAsync()
        {
            string groupsEndpoint = $"{BaseApiUrl}/Groups";
            return await _httpClient.GetFromJsonAsync<List<Group>>(groupsEndpoint);
        }

        public async Task AddGroupAsync(Group newGroup)
        {
            string endpoint = $"{BaseApiUrl}/Groups";
            var response = await _httpClient.PostAsJsonAsync(endpoint, newGroup);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Nie udało się dodać nowej grupy.");
            }
        }

        public async Task AddTaskAsync(TodoTask newTask)
        {
            string endpoint = $"{BaseApiUrl}/Tasks";
            var response = await _httpClient.PostAsJsonAsync(endpoint, newTask);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Nie udało się dodać nowego zadania.");
            }
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            string endpoint = $"{BaseApiUrl}/Tasks/{taskId}";
            var response = await _httpClient.DeleteAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Nie udało się usunąć zadania.");
            }
        }

        public async Task DeleteGroupAsync(int groupId)
        {
            string endpoint = $"{BaseApiUrl}/Groups/{groupId}";
            var response = await _httpClient.DeleteAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Nie udało się usunąć grupy.");
            }
        }

        public async Task UpdateTaskAsync(TodoTask updatedTask)
        {
            string endpoint = $"{BaseApiUrl}/Tasks/{updatedTask.TaskId}"; // PUT wymaga TaskId w URL
            var response = await _httpClient.PutAsJsonAsync(endpoint, updatedTask);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Nie udało się zaktualizować zadania.");
            }
        }

        public async Task UpdateGroupAsync(Group updatedGroup)
        {
            string endpoint = $"{BaseApiUrl}/Groups/{updatedGroup.GroupId}"; // PUT wymaga GroupId w URL
            var response = await _httpClient.PutAsJsonAsync(endpoint, updatedGroup);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Nie udało się zaktualizować grupy.");
            }
        }
    }
}