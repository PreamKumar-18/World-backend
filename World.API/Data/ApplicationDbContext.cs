    using Microsoft.EntityFrameworkCore;
    using World.API.Models;

    namespace World.API.Data
    {
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {
                
            }
            public DbSet<Country> Countries { get; set; }
        }
    }
