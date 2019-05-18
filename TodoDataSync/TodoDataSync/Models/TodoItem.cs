
using System;

namespace TodoDataSync.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool Done { get; set; }
    }
}

