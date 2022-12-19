using Microsoft.AspNetCore.Identity;

namespace PlannerAPI.Models {
    public class PlannerUser : IdentityUser {
        public string ProfileImage { get; set; }
    }
}
