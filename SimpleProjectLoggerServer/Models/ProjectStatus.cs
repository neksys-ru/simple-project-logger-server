using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProjectLoggerServer.Models
{
    public class ProjectStatus:Entity
    {
        public string Name { get; set; }
        public List<Project> Projects { get; set; } = new List<Project>();
    }
}
