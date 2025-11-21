using System.ComponentModel.DataAnnotations;

namespace FeedingCampaigns.Api.Dtos.Distributions
{
    public class DistributionLineCreateDto
    {
        [Required]
        public int BeneficiaryFamilyId { get; set; }

        [Range(1, int.MaxValue)]
        public int MealsDelivered { get; set; }
    }

    public class DistributionBatchCreateDto
    {
        [Required]
        public int CampaignId { get; set; }

        public int? BranchId { get; set; }

        [Required]
        public DateTime DistributionDate { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }

        [Required]
        public List<DistributionLineCreateDto> Lines { get; set; } = new();
    }

    public class DistributionBatchSummaryDto
    {
        public int Id { get; set; }

        public int CampaignId { get; set; }

        public DateTime DistributionDate { get; set; }

        public int TotalMealsDelivered { get; set; }

        public int FamiliesServed { get; set; }
    }
}
