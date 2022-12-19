using Microsoft.AspNetCore.Identity;

namespace PlannerAPI.Types {
    public class AuthResponse : BasicResult {
        public string Token { get; set; }

        public string ProfileImage { get; set; }
        public AuthResponse(string tokenStr, string profileImage, string result) : base(result) {
            Token = tokenStr;
            ProfileImage = profileImage;
        }
    }
}
