using System;
using System.Collections.Generic;

namespace COMP2001_Assessment2.Models
{
    public partial class Profile
    {
        public Profile()
        {
            Achievements = new HashSet<Achievement>();
            Activities = new HashSet<Activity>();
            FollowRelationshipFollowedProfiles = new HashSet<FollowRelationship>();
            FollowRelationshipFollowerProfiles = new HashSet<FollowRelationship>();
            Trails = new HashSet<Trail>();
        }

        public int ProfileId { get; set; }
        public int? UserId { get; set; }
        public string? ProfileName { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? ProfileBirthday { get; set; }
        public string? Bio { get; set; }
        public DateTime? JoinDate { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Achievement> Achievements { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<FollowRelationship> FollowRelationshipFollowedProfiles { get; set; }
        public virtual ICollection<FollowRelationship> FollowRelationshipFollowerProfiles { get; set; }
        public virtual ICollection<Trail> Trails { get; set; }
    }
}
