using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProjectLoggerServer.Models
{
    public class TaggedOwnedEntity:OwnedEntity
    {
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
