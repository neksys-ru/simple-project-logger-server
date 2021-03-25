using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProjectLoggerServer.Models
{
    public class Document: TaggedOwnedEntity
    {
        public string Path { get; set; }
        public string Title { get; set; }
        public int? ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
