using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlannerAPI.InputModels;
using PlannerAPI.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using PlannerAPI.Services;

namespace PlannerAPI.Controllers {
    [Route("signup")]
    [ApiController]
    public class SignupController : ControllerBase {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SMTPEmailSender _smtpSender;
        public SignupController(UserManager<ApplicationUser> userManager, SMTPEmailSender smtpSender) {
            _userManager = userManager;
            _smtpSender = smtpSender;
        }

        //If returns true, then it means user was successfully created
        [HttpPost]
        public async Task<bool> createNewUser([FromBody] SignupModel userData) {
            ApplicationUser user = new ApplicationUser { 
                Email = userData.Email, 
                PasswordHash = userData.PasswordHash, 
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                BackgroundImage = "",
                ProfileImage = ""
            };

            //creates the user and returns the result
            IdentityResult result = await _userManager.CreateAsync(user);

            if (result.Succeeded) {
                string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                _smtpSender.SendVerificationEmail(userData.FirstName, userData.LastName, userData.Email, code);
                return true;
            }
            return false;
            
        }

        [HttpGet("resend/{email}")]
        public async Task<bool> ResendEmail([FromRoute] string email) {
            //Find the user in the database
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            if (user == null) {
                return false;
            }
            //Generate new code and resend email
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            _smtpSender.SendVerificationEmail(user.FirstName, user.LastName, email, code);
            return true;
        }

        [HttpGet("verify/{email}/{code}")]
        public async Task<bool> VerifyUser([FromRoute] string email, [FromRoute] string code) {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            if (user == null) {
                return false;
            }
            //Confirm email
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded) {
                return true;
            }
            return false;
        }

    }
}
