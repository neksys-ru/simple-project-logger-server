using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using SimpleProjectLoggerServer.Models;
using SimpleProjectLoggerServer.Service;

namespace SimpleProjectLoggerServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        UserContext db;
        IPassHasher _hasher;
        public AuthController(UserContext context)
        {
            db = context;
            _hasher = new PasswordHasher();
            if (!db.Users.Any())
            {
                Role admin = new Role { Name = "admin" };
                Role ruser = new Role { Name = "user" };
                db.Roles.Add(admin);
                User user = new User { Name = "Tom", Email = "admin@neksys.ru", Password = _hasher.GeneratePasswordHash("secret"), Role = admin };
                User user2 = new User { Name = "Tomas", Email = "info@neksys.ru", Password = _hasher.GeneratePasswordHash("secret"), Role = ruser };
                db.Users.Add(user);
                db.Users.Add(user2);
                db.SaveChanges();
            }
        }

        [HttpPost("token")]
        public IActionResult Token(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var identity = GetIdentity(model.Username, model.Password);
                if (identity == null)
                {
                    return BadRequest(new { errorText = "Неверный пользователь или пароль" });
                }

                var now = DateTime.UtcNow;
                // создаем JWT-токен
                var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        notBefore: now,
                        claims: identity.Claims,
                        expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                var response = new
                {
                    access_token = encodedJwt,
                    username = identity.Name
                };

                return Ok(response);
            }
            else {
                return BadRequest();
            }
           
        }
        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User person = db.Users.FirstOrDefault(x => x.Email == username);
            if (person != null)
            {
                if (_hasher.VerifyPasswordHash(password, person.Password))
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Email),
                    new Claim(ClaimTypes.NameIdentifier, person.Id.ToString()),
                    new Claim(ClaimTypes.Name, person.Name),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role?.Name)
                };
                    ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                    return claimsIdentity;
                }
                else {
                    return null;
                }
                
            }

            // если пользователя не найдено
            return null;
        }
    }
}
