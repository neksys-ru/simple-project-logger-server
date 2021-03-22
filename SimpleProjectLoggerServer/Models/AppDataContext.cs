using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SimpleProjectLoggerServer.Models
{
    public class AppDataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public AppDataContext(DbContextOptions<AppDataContext> options):base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
