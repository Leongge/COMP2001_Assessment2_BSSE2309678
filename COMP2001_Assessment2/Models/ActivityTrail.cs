using System;
using System.Collections.Generic;

namespace COMP2001_Assessment2.Models
{
    public partial class ActivityTrail
    {
        public int ActivityTrailId { get; set; }
        public int? ActivityId { get; set; }
        public int? TrailId { get; set; }
        public int? ActivitySequenceNumber { get; set; }
        public int? TrailSequenceNumber { get; set; }

        public virtual Activity? Activity { get; set; }
        public virtual Trail? Trail { get; set; }
    }
}
