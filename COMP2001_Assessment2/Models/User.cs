using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace COMP2001_Assessment2.Models
{
    public partial class User
    {
        public User()
        {
            Profiles = new HashSet<Profile>();
        }

        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("registrationDate")]
        public DateTime RegistrationDate { get; set; }

        public virtual ICollection<Profile> Profiles { get; set; }
    }
}
