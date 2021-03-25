using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimpleProjectLoggerServer.Models
{
    public class Role:Entity
    {
        public string Name { get; set;  }
        public int Order { get; set; }
        [JsonIgnore]
        public List<User> Users { get; set; } = new List<User>();

    }
}
