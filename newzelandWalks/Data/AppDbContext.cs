using Microsoft.EntityFrameworkCore;
using newzelandWalks.Models.Domain;

namespace newzelandWalks.Data
{
    public class AppDbContext:DbContext
    {
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public AppDbContext(DbContextOptions db) : base(db)
        {

        }
    }
}
