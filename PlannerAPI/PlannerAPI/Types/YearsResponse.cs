using PlannerAPI.Models;

namespace PlannerAPI.Types {
    public class YearsResponse : BasicResult {
        public List<Year> Years {get; set;}
        public YearsResponse(string result, List<Year> years): base(result) {
            Years = years;
        }
    }
}
