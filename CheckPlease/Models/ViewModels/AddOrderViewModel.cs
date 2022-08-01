using System.Collections.Generic;

namespace CheckPlease.Models.ViewModels
{
    public class AddOrderViewModel
    {
        public GroupOrderUser Gou { get; set; }
        public List<FoodItem> Menu { get; set; }
    }
}
