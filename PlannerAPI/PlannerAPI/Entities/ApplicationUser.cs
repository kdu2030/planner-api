using Microsoft.AspNetCore.Identity;

namespace PlannerAPI.Entities {
    public class ApplicationUser : IdentityUser {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        
        //This is the file path to the Profile Image
        [PersonalData]
        public string ProfileImage { get; set; }

        [PersonalData]
        public string BackgroundImage { get; set; }

    }
}
