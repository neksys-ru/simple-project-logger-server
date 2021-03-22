using SimpleProjectLoggerServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProjectLoggerServer.Interfaces
{
    interface IOwned
    {
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
