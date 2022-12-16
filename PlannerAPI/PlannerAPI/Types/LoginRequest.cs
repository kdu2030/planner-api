namespace PlannerAPI.Types {
    public class LoginRequest {
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public bool isValid() {
            if(Email == null || PasswordHash == null) {
                return false;
            }
            else if (Email.Length == 0 || PasswordHash.Length == 0) {
                return false;
            }
            return true;
        }
    }
}
