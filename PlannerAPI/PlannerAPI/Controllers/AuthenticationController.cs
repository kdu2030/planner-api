using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PlannerAPI.Services.Authentication;

namespace PlannerAPI.Controllers {
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : Controller {
        private UserManager<IdentityUser> _userManager;
        private TokenGenerator _tokenGenerator;

        public AuthenticationController(UserManager<IdentityUser> userManager, TokenGenerator tokenGenerator) {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request) {
            if (!request.IsValid()) {
                return BadRequest(new { result = "None of the fields should be blank" });
            }
            // Does a user already exist with this username?
            IdentityUser existingUser = await _userManager.FindByNameAsync(request.Username);
            if (existingUser != null) {
                return BadRequest(new {result = "User Email Exists"});
            }

            IdentityUser user = new IdentityUser { Email = request.Email, UserName = request.Username, PasswordHash = request.PasswordHash};
            await _userManager.CreateAsync(user);
            return Ok(new { result = _tokenGenerator.generate(user) });
        }
    }

    public class SignupRequest {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public bool IsValid() {
            if (Username == null || Email == null || PasswordHash == null) {
                return false;
            }
            else if(Username.Length == 0 || Email.Length == 0 || PasswordHash.Length == 0) {
                return false;
            }
            return true;
        }
        
    }
}
