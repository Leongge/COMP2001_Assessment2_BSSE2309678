using System;
using System.Collections.Generic;

namespace COMP2001_Assessment2.Models
{
    public partial class User
    {
        public User()
        {
            Profiles = new HashSet<Profile>();
        }

        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public virtual ICollection<Profile> Profiles { get; set; }
    }
}
