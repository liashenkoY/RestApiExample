using Microsoft.EntityFrameworkCore;
using RstApiExample.Entities;

namespace RstApiExample.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
}
