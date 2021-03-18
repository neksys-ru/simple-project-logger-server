using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProjectLoggerServer
{
    public class AuthOptions
    {
        public const string ISSUER = "SPLServer";
        public const string AUDIENCE = "SPLClient"; 
        const string KEY = "mysupersecret_secretkey!123";   
        public const int LIFETIME = 1; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
