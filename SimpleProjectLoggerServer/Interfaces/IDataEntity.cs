using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleProjectLoggerServer.Models;

namespace SimpleProjectLoggerServer.Interfaces
{
    interface IDataEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
