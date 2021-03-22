using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleProjectLoggerServer.Models;
using SimpleProjectLoggerServer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimpleProjectLoggerServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class UserController : ControllerBase
    {

        AppDataContext db;
        IPassHasher _hasher;
        public UserController(AppDataContext context)
        {
            db = context;
            _hasher = new PasswordHasher();
            /*if (!db.Users.Any())
            {
                Role admin = new Role { Name = "admin"};
                Role ruser = new Role { Name = "user"};
                db.Roles.Add(admin);
                User user = new User { Name = "Tom", Email="admin@neksys.ru", Password = _hasher.GeneratePasswordHash("secret"),Role=admin };
                User user2 = new User { Name = "Tomas",Email="info@neksys.ru", Password = _hasher.GeneratePasswordHash("secret"), Role=ruser };
                db.Users.Add(user);
                db.Users.Add(user2);
                db.SaveChanges();
            }*/
        }

        // GET: api/<UserController>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await db.Users.ToListAsync();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            User user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return NotFound();
            return new ObjectResult(user);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            user.Password = _hasher.GeneratePasswordHash(user.Password);
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Put(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            if (!db.Users.Any(x => x.Id == user.Id))
            {
                return NotFound();
            }
            //TODO password update logic
            db.Update(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int id)
        {
            User user = db.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }
    }
}
