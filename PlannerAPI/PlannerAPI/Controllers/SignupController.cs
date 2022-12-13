using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PlannerAPI.Services.Authentication;

namespace PlannerAPI.Controllers {
    [ApiController]
    public class SignupController : Controller {
        private UserManager<IdentityUser> _userManager;
        private TokenGenerator _tokenGenerator;

        public SignupController(UserManager<IdentityUser> userManager, TokenGenerator tokenGenerator) {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost]
        public async Task<IActionResult> Signup([FromBody] string email, [FromBody] string username, [FromBody] string passwordHash) {
            // Does a user already exist with this email?
            IdentityUser existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null) {
                BadRequest(new {result = "User Email Exists"});
            }
            IdentityUser user = new IdentityUser { Email = email, UserName = username, PasswordHash = passwordHash};
            await _userManager.CreateAsync(user);
            return Ok(_tokenGenerator.generate(user));
        }
    }
}
