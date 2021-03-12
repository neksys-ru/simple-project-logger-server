using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace SimpleProjectLoggerServer.Service
{
    public class PasswordHasher : IPassHasher
    {
        private string _salt;
        public PasswordHasher() {
            _salt = "s[pdo";
        }

        public string GeneratePasswordHash(string password)
        {
            string salted = password + _salt;
            return BCrypt.Net.BCrypt.EnhancedHashPassword(salted);
            //return BCrypt.Net.BCrypt.HashPassword(salted);
        }

        public bool VerifyPasswordHash(string password, string hashed)
        {
            string salted = password + _salt;
            return BCrypt.Net.BCrypt.EnhancedVerify(salted, hashed);
            //return BCrypt.Net.BCrypt.Verify(salted, hashed);
        }
    }
}
