using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProjectLoggerServer.Service
{
    interface IPassHasher
    {
        public string GeneratePasswordHash(string password);
        public bool VerifyPasswordHash(string password, string hashed);
    }
}
