using System;
using System.Collections.Generic;

namespace COMP2001_Assessment2.Models
{
    public partial class FollowRelationship
    {
        public int FollowId { get; set; }
        public int? FollowerProfileId { get; set; }
        public int? FollowedProfileId { get; set; }
        public DateTime? FollowDate { get; set; }

        public virtual Profile? FollowedProfile { get; set; }
        public virtual Profile? FollowerProfile { get; set; }
    }
}
