﻿namespace TodoApp.Models
{
    public class TodoTask
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public int GroupId { get; set; }
        public string? GroupName { get; set; }
    }
}