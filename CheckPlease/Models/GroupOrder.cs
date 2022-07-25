using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckPlease.Models
{
    public class GroupOrder
    {
        public int Id { get; set; }
        [Required]
        public int OwnerId { get; set; }
        [Required]
        public int RestaurantId { get; set; }
        [Required]
        public bool IsReady { get; set; }
        public List<UserProfile> GroupMember { get; set; }

    }
}
