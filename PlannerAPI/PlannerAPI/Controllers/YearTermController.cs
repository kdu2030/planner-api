using Microsoft.AspNetCore.Mvc;
using PlannerAPI.Services.Authentication;
using PlannerAPI.Models;
using PlannerAPI.Types;

namespace PlannerAPI.Controllers {
    [Route("year")]
    public class YearTermController : ControllerBase {
        private TokenGenerator _tokenGenerator;
        private AppDbContext _appDbContext;

        public YearTermController(TokenGenerator tokenGenerator, AppDbContext appDbContext) : base() {
            _tokenGenerator = tokenGenerator;
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public IActionResult GetYears([FromHeader] string token) {
            string userId = _tokenGenerator.Validate(token);
            if (userId.Length == 0) {
                return BadRequest(new BasicResult("User not found"));
            }
            List<Year> years = _appDbContext.Years.Where(year => year.PlannerUserId == userId).ToList();
            return Ok(new YearsResponse("Success", years));
        }
    }
}
