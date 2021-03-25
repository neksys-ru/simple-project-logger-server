using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProjectLoggerServer.Models
{
    public class RefreshToken:OwnedEntity
    {

        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public string IpAddress { get; set; }
    }
    public class RefreshTokenModel
    {
        [Required]
        public string Token { get; set; }
    }
}
