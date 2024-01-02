using System;
using System.Collections.Generic;

namespace COMP2001_Assessment2.Models
{
    public partial class Activity
    {
        public Activity()
        {
            ActivityTrails = new HashSet<ActivityTrail>();
        }

        public int ActivityId { get; set; }
        public int? ProfileId { get; set; }
        public string? ActivityType { get; set; }
        public string? ActivityDescription { get; set; }
        public DateTime? ActivityDate { get; set; }

        public virtual Profile? Profile { get; set; }
        public virtual ICollection<ActivityTrail> ActivityTrails { get; set; }
    }
}
