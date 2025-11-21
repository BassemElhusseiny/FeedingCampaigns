using System.ComponentModel.DataAnnotations;

namespace FeedingCampaigns.Api.Models
{
    public enum InventoryTransactionType
    {
        InboundFromDonation = 1,
        OutboundToDistribution = 2,
        Adjustment = 3
    }

    public class InventoryTransaction
    {
        public int Id { get; set; }

        [Required]
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; } = null!;

        [Required]
        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; } = null!;

        public InventoryTransactionType Type { get; set; }

        public decimal QuantityDelta { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? DonationId { get; set; }
        public Donation? Donation { get; set; }

        public int? DistributionBatchId { get; set; }
        public DistributionBatch? DistributionBatch { get; set; }
    }
}
