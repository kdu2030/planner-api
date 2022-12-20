namespace PlannerAPI.Models {
    public class Project {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string PlannerUserId { get; set; }
        public PlannerUser User { get; set; }

    }
}
