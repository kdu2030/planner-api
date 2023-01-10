using System.Diagnostics.CodeAnalysis;

namespace PlannerAPI.Types {
    public class BasicResult {
        public object Result { get; set; }

        public BasicResult(object Result) {
            this.Result = Result;
        }


        public override bool Equals([AllowNull] object other) {
            if(other is not BasicResult) {
                return false;
            }
            if (other == null) {
                return false;
            }
            BasicResult otherResult = (BasicResult) other;
            return Result == otherResult.Result;
        }
    }
}
