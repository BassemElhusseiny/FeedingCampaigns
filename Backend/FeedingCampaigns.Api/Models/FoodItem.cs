using System.ComponentModel.DataAnnotations;

namespace FeedingCampaigns.Api.Models
{
    public class FoodItem
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string? Unit { get; set; } = "kg";

        public int? CaloriesPerUnit { get; set; }

        public bool IsPerishable { get; set; }

        public ICollection<DonationItem>? DonationItems { get; set; }
        public ICollection<InventoryTransaction>? InventoryTransactions { get; set; }
    }
}
