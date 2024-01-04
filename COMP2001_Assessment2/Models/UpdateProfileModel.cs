namespace COMP2001_Assessment2.Models
{
    public class UpdateProfileModel
    {
        public int ProfileId { get; set; }
        public int UserId { get; set; }
        public string ProfileName { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? ProfileBirthday { get; set; }
        public string Bio { get; set; }
        public DateTime? JoinDate { get; set; }
    }
}
