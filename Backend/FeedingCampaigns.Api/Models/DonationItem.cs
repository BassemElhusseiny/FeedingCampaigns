using System.ComponentModel.DataAnnotations;

namespace FeedingCampaigns.Api.Models
{
    public class DonationItem
    {
        public int Id { get; set; }

        [Required]
        public int DonationId { get; set; }
        public Donation Donation { get; set; } = null!;

        [Required]
        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; } = null!;

        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }

        [MaxLength(50)]
        public string? Unit { get; set; }
    }
}
