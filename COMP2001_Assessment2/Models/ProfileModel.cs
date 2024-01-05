using System;

namespace COMP2001_Assessment2.Models
{
    public class ProfileModel
    {
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? JoinDate { get; set; }
        public int FollowerCount { get; set; }
        public int FollowedCount { get; set; }
    }
}
