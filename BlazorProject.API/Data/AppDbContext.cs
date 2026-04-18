using BlazorProject.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorProject.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
    }
}
