using System.ComponentModel.DataAnnotations;

namespace PlannerAPI.Models {
    public class Term {
        public int Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public int YearId { get; set; }

        public Year Year { get; set; }
        public List<Course> Courses { get; set; }
    }
}
