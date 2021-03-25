using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleProjectLoggerServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProjectLoggerServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class RoleController : ControllerBase
    {
        AppDataContext db;
        public RoleController(AppDataContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> Get()
        {
            return await db.Roles.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> Get(int id)
        {
            var role = await db.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if(role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        [HttpPost]
        public async Task<ActionResult<Role>> Post(Role role)
        {
            if(role == null)
            {
                return BadRequest();
            }
            db.Roles.Add(role);
            await db.SaveChangesAsync();
            return Ok(role);
        }
        [HttpPut("id")]
        public async Task<ActionResult<Role>> Put(Role role)
        {
            if (role == null)
            {
                return BadRequest();
            }
            if (!db.Roles.Any(r => r.Id == role.Id))
            {
                return NotFound();
            }
            db.Roles.Update(role);
            await db.SaveChangesAsync();
            return Ok(role);
        }
        [HttpDelete("id")]
        public async Task<ActionResult<Role>> Delete(int id)
        {
            var role = await db.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (role == null)
            {
                return NotFound();
            }
            db.Roles.Remove(role);
            await db.SaveChangesAsync();
            return Ok(role);
        }
    }
}
