using System;
using System.Collections.Generic;

namespace COMP2001_Assessment2.Models
{
    public partial class Trail
    {
        public Trail()
        {
            ActivityTrails = new HashSet<ActivityTrail>();
        }

        public int TrailId { get; set; }
        public int? ProfileId { get; set; }
        public string? TrailName { get; set; }
        public string? TrailDescription { get; set; }
        public DateTime? TrailStartDate { get; set; }

        public virtual Profile? Profile { get; set; }
        public virtual ICollection<ActivityTrail> ActivityTrails { get; set; }
    }
}
