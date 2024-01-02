using System;
using System.Collections.Generic;

namespace COMP2001_Assessment2.Models
{
    public partial class Achievement
    {
        public int AchievementId { get; set; }
        public int? ProfileId { get; set; }
        public string? AchievementName { get; set; }
        public string? AchievementDescription { get; set; }
        public DateTime? AchievementDate { get; set; }

        public virtual Profile? Profile { get; set; }
    }
}
