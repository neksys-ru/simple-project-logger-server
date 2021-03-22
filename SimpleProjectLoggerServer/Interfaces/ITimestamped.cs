using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProjectLoggerServer.Interfaces
{
    interface ITimestamped
    {
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
