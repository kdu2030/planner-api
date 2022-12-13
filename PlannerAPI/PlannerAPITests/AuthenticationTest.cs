using PlannerAPI.Controllers;
using Microsoft.AspNetCore.Identity;

namespace PlannerAPITests {
    public class AuthenticationTest {

        
        [Fact]
        public void TestSignup_AllFieldsFilled() {
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>();
            AuthenticationController authController = new AuthenticationController()
            
        }
    }
}