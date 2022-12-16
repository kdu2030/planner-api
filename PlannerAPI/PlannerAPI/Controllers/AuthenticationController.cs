using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PlannerAPI.Services.Authentication;
using PlannerAPI.Types;

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
                return BadRequest(new BasicResult("Empty Fields"));
            }
            // Does a user already exist with this username?
            IdentityUser existingUser = await _userManager.FindByNameAsync(request.Username);
            if (existingUser != null) {
                return BadRequest(new BasicResult("User with username exists"));
            }
            IdentityUser existingUserEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingUserEmail != null) {
                return BadRequest(new BasicResult("User with email exists"));
            }

            IdentityUser user = new IdentityUser { Email = request.Email, UserName = request.Username, PasswordHash = request.PasswordHash};
            await _userManager.CreateAsync(user);
            return Ok(new BasicResult(_tokenGenerator.Generate(user)));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request) {
            if (!request.isValid()) {
                return BadRequest(new BasicResult("Empty Fields"));
            }
            IdentityUser user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) {
                return Unauthorized(new BasicResult("User Not Found"));
            } else if (user.PasswordHash == request.PasswordHash) {
                return Ok(new BasicResult(_tokenGenerator.Generate(user)));
            }
            return Unauthorized(new BasicResult("Incorrect password"));
        }
    }
}
