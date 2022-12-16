namespace PlannerAPI.Types {
    public class SignupRequest {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public bool IsValid() {
            if (Username == null || Email == null || PasswordHash == null) {
                return false;
            } else if (Username.Length == 0 || Email.Length == 0 || PasswordHash.Length == 0) {
                return false;
            }
            return true;
        }

    }
}
