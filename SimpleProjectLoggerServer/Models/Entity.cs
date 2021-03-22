﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProjectLoggerServer.Models
{
    public class Entity
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}