using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlannerAPI.Entities;

namespace PlannerAPI {
    public class AppDbContext : IdentityDbContext<ApplicationUser> {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){
        }


    }
}
