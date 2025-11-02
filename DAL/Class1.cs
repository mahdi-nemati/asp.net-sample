using Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class CourseDBContext : DbContext
    {
        public CourseDBContext(DbContextOptions<CourseDBContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

    }
}
