using System;

namespace GitHubWebProject.Models
{
    public class Issue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Number { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

    }
}