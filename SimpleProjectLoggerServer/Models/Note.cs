using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProjectLoggerServer.Models
{
    public class Note : TaggedOwnedEntity
    {
        public string Content { get; set; }
        public int? ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
