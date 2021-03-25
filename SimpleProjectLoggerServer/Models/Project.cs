using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProjectLoggerServer.Models
{
    public class Project:Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ProjectStatus> ProjectStatuses { get; set; } = new List<ProjectStatus>();
        public List<User> Users { get; set; } = new List<User>();
        public List<Note> Notes { get; set; } = new List<Note>();
        public List<Document> Documents { get; set; } = new List<Document>();
    }
}
