using System.Collections.Generic;

namespace CheckPlease.Models.ViewModels
{
    public class CreateGroupOrderViewModel
    {
        public GroupOrder GroupOrder { get; set; }
        public List<Restaurant> Restaurants { get; set; }
        public List<UserProfile> UserProfiles { get; set; }
        public List<int> SelectedUserIds { get; set; }
    }
}
