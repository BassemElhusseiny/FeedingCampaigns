using System.ComponentModel.DataAnnotations;

namespace FeedingCampaigns.Api.Models
{
    public class DistributionLine
    {
        public int Id { get; set; }

        [Required]
        public int BatchId { get; set; }
        public DistributionBatch Batch { get; set; } = null!;

        [Required]
        public int BeneficiaryFamilyId { get; set; }
        public BeneficiaryFamily BeneficiaryFamily { get; set; } = null!;

        public int MealsDelivered { get; set; }

        public int? DeliveredByUserId { get; set; }
        public User? DeliveredByUser { get; set; }
    }
}
