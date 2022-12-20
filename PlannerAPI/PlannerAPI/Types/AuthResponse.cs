using PlannerAPI.Models;

namespace PlannerAPI.Types {
    public class AuthResponse : BasicResult {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string ProfileImage { get; set; }
        
        public AuthResponse(PlannerUser user, string tokenStr, string result) : base(result) {
            Username = user.UserName;
            Email = user.Email;
            ProfileImage = user.ProfileImage;
            Token = tokenStr;
        }
    }
}
