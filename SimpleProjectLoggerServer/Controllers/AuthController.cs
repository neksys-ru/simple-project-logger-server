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
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace SimpleProjectLoggerServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        AppDataContext db;
        IPassHasher _hasher;
        public AuthController(AppDataContext context)
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
                
                User person = db.Users.Include(x=>x.Role).FirstOrDefault(x => x.Email == model.Username);
                if (person == null) return BadRequest(new { errorText = "Неверный пользователь или пароль" });
                if (!_hasher.VerifyPasswordHash(model.Password, person.Password)) return BadRequest(new { errorText = "Неверный пользователь или пароль" });

                var identity = GetIdentity(person);

                var encodedJwt = generateJwtToken(identity);
                var refreshToken = generateRefreshToken(ipAddress());
                db.RefreshTokens.Add(refreshToken);
                person.RefreshTokens.Add(refreshToken);
                //db.Update(person);
                db.SaveChanges();
                var response = new
                {
                    access_token = encodedJwt,
                    refresh_token = refreshToken.Token,
                    username = identity.Name,
                    user_id = person.Id
                };

                return Ok(response);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost("refresh")]
        public IActionResult RefreshToken(RefreshTokenModel model)
        {
            if (ModelState.IsValid)
            {
                //var user = db.Users.SingleOrDefault(u => u.RefreshTokens.Any(t=> t.Token ==model.Token));
                var refreshToken = db.RefreshTokens.Include(t=>t.User.Role).FirstOrDefault(t => t.Token == model.Token);

                // return null if no user found with token
                if (refreshToken == null) return BadRequest(new { errorText = "Token is missing" });

                var user = refreshToken.User;

                // return null if token is no longer active
                if (refreshToken.IsExpired) return BadRequest(new { errorText = "Token is expired" });

                // replace old refresh token with a new one and save
                var newRefreshToken = generateRefreshToken(ipAddress());

                db.Remove(refreshToken);
                db.RefreshTokens.Add(newRefreshToken);
                user.RefreshTokens.Add(newRefreshToken);
                //db.Update(user);
                db.SaveChanges();

                // generate new jwt
                var identity = GetIdentity(user);
                var jwtToken = generateJwtToken(identity);

                var response = new
                {
                    access_token = jwtToken,
                    refresh_token = newRefreshToken.Token,
                    username = identity.Name,
                    user_id = user.Id
                };

                return Ok(response);
            }
            else
            {
                return BadRequest(new { errorText = "Wrong data supported" });
            }
        }
        private ClaimsIdentity GetIdentity(User person)
        {

            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role?.Name)
                };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;

        }
        private string generateJwtToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        private RefreshToken generateRefreshToken(string ipAddress)
        {
            var now = DateTime.UtcNow;
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Created = now,
                    Expires = now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME_REFRESH)),
                    IpAddress = ipAddress
                };
            }
        }
        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
