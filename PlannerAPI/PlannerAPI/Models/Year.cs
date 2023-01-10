using System.ComponentModel.DataAnnotations;

namespace PlannerAPI.Models {
    public class Year {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public List<Term> Terms { get; set; }

        public string PlannerUserId { get; set; }

        public PlannerUser User { get; set; }

    }
}
