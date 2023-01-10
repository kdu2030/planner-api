using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PlannerAPI.Models {
    public class AppDbContext : IdentityDbContext<PlannerUser> {
        public DbSet<Course> Courses { get; set; }
        
        public DbSet<Term> Terms { get; set; }

        public DbSet<Year> Years { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.Entity<Course>()
                .Property(e => e.Days)
                .HasConversion(
                    daysList => string.Join(",", daysList),
                    daysStr => daysStr.Split(",", StringSplitOptions.None).ToList()
                );

            builder.Entity<Course>()
                    .HasOne(e => e.Term)
                    .WithOne()
                    .OnDelete(DeleteBehavior.NoAction);
            
        }
    }
}
