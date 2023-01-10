﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PlannerAPI.Services.Authentication;
using PlannerAPI.Types;
using PlannerAPI.Models;

namespace PlannerAPI.Controllers {
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : Controller {
        private UserManager<PlannerUser> _userManager;
        private TokenGenerator _tokenGenerator;

        public AuthenticationController(UserManager<PlannerUser> userManager, TokenGenerator tokenGenerator) {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request) {
            if (!request.IsValid()) {
                return Ok(new BasicResult("Empty Fields"));
            }

            // Does a user already exist with this username?
            PlannerUser existingUser = await _userManager.FindByNameAsync(request.Username);
            if (existingUser != null) {
                return Ok(new BasicResult("User with username exists"));
            }

            //Does a user already exist with this user?
           PlannerUser existingUserEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingUserEmail != null) {
                return Ok(new BasicResult("User with email exists"));
            }

            PlannerUser user = new PlannerUser { Email = request.Email, UserName = request.Username, PasswordHash = request.PasswordHash, ProfileImage = "profile.png"};
            await _userManager.CreateAsync(user);
            string token = _tokenGenerator.Generate(user);
            
            return Ok(new AuthResponse(user, token, "User created successfully"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request) {
            if (!request.isValid()) {
                return Ok(new BasicResult("Empty Fields"));
            }
            PlannerUser user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) {
                return Ok(new BasicResult("User Not Found"));
            } else if (user.PasswordHash != request.PasswordHash) {
                return Ok(new BasicResult("Incorrect password"));
            }
            string token = _tokenGenerator.Generate(user);
            return Ok(new AuthResponse(user, token, "Successful login"));            
        }
    }
}
