using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimpleProjectLoggerServer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public int? RoleId { get; set; }
        public Role Role { get; set; }
        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    }
    public class LoginViewModel {
        [Required(ErrorMessage = "Вводить обязательно")]
        [EmailAddress(ErrorMessage = "Не правильный имейл")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; }
    }

}
