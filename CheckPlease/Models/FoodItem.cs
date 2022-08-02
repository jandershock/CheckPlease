namespace CheckPlease.Models
{
    public class FoodItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int RestaurantId { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
    }
}
