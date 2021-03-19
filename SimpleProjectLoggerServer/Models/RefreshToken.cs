using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProjectLoggerServer.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public string IpAddress { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
    }
    public class RefreshTokenModel
    {
        [Required]
        public string Token { get; set; }
    }
}
