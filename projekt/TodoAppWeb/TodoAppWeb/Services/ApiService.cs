using System.Net.Http.Json;
using TodoAppWeb.Models;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private const string BaseApiUrl = "http://localhost:5279/api";

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<GroupModel>> GetGroupsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<GroupModel>>($"{BaseApiUrl}/Groups");
    }

    public async Task<List<TaskModel>> GetTasksAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<TaskModel>>($"{BaseApiUrl}/Tasks");
    }

    public async Task AddGroupAsync(GroupModel group)
    {
        await _httpClient.PostAsJsonAsync($"{BaseApiUrl}/Groups", group);
    }

    public async Task AddTaskAsync(TaskModel task)
    {
        await _httpClient.PostAsJsonAsync($"{BaseApiUrl}/Tasks", task);
    }

    public async Task DeleteGroupAsync(int id)
    {
        await _httpClient.DeleteAsync($"{BaseApiUrl}/Groups/{id}");
    }

    public async Task DeleteTaskAsync(int id)
    {
        await _httpClient.DeleteAsync($"{BaseApiUrl}/Tasks/{id}");
    }

    public async Task UpdateTaskAsync(TaskModel task)
    {
        await _httpClient.PutAsJsonAsync($"{BaseApiUrl}/Tasks/{task.TaskId}", task);
    }

    public async Task UpdateGroupAsync(GroupModel group)
    {
        await _httpClient.PutAsJsonAsync($"{BaseApiUrl}/Groups/{group.GroupId}", group);
    }
}