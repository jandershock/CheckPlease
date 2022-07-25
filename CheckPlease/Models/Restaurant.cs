using System.ComponentModel.DataAnnotations;

namespace CheckPlease.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
