using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProjectLoggerServer.Models
{
    public class OwnedEntity:Entity
    {
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
