namespace CheckPlease.Models
{
    public class GroupOrderUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GroupOrderId { get; set; }
        public bool HasOrdered { get; set; } = false;
    }
}
