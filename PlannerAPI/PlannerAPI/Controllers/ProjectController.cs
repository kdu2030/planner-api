using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PlannerAPI.Models;
using PlannerAPI.Services.Authentication;
using PlannerAPI.Types;

namespace PlannerAPI.Controllers {
    [Route("api/Course")]
    [ApiController]
    public class CourseController : ControllerBase {
        private UserManager<PlannerUser> _userManager;
        private TokenGenerator _tokenGenerator;
        private AppDbContext _context;

        public CourseController(UserManager<PlannerUser> userManager, TokenGenerator tokenGenerator, AppDbContext context) {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _context = context;
        }

        [HttpGet("Courses")]
        public async Task<IActionResult> GetAllCourses([FromHeader] string token) {
            string userId = _tokenGenerator.Validate(token);
            if(userId == "") {
                return Unauthorized(new BasicResult("Invalid token"));
            }
            PlannerUser user = await _userManager.FindByIdAsync(userId);
            return Ok(new BasicResult(user.Courses));
        }

        //public async Task<IActionResult> CreateCourse([FromHeader] string token) {
        //    string userId = _tokenGenerator.Validate(token);
        //    if(userId == "") {
        //        return Unauthorized(new BasicResult("Invalid token"));
        //    }
            
        //}

        

    }
}
