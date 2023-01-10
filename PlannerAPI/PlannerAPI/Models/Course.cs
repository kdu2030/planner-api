using System.ComponentModel.DataAnnotations;

namespace PlannerAPI.Models {
    public class Course {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public string Location { get; set; }

        public bool IsArchived { get; set; }

        public List<string> Days { get; set; }

        //Should be in UTC
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        public int HoursSpent { get; set; }

        public int MinutesSpent { get; set; }

        public string PlannerUserId { get; set; }

        public PlannerUser User { get; set; }

        public int TermId { get; set; }

        public Term Term { get; set; }

    }
}
