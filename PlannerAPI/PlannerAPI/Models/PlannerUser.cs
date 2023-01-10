using Microsoft.AspNetCore.Identity;

namespace PlannerAPI.Models {
    public class PlannerUser : IdentityUser {
        public string ProfileImage { get; set; }
        public List<Course> Courses { get; set; }

        public List<Year> Years { get; set; }

    }
}
