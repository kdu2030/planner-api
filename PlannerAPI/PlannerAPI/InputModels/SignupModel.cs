namespace PlannerAPI.InputModels {
    public class SignupModel {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public SignupModel(string firstName, string lastName, string email, string passwordHash) {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
            
        }
    }
}
