using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProjectLoggerServer.Models
{
    public class Tag: OwnedEntity
    {
        public string Name { get; set; }
        public int? RoleId { get; set; }
        public Role Role { get; set; }
        public List<Document> Documents { get; set; } = new List<Document>();
        public List<Note> Notes { get; set; } = new List<Note>();
    }
}
