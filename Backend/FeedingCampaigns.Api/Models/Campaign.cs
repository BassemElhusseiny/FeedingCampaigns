using System.ComponentModel.DataAnnotations;

namespace FeedingCampaigns.Api.Models
{
    public enum CampaignStatus
    {
        Draft = 0,
        Planned = 1,
        Active = 2,
        Paused = 3,
        Completed = 4,
        Cancelled = 5
    }

    public class Campaign
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required, MaxLength(2000)]
        public string Description { get; set; } = null!;

        [Required]
        public int BranchId { get; set; }
        public Branch Branch { get; set; } = null!;

        [Required]
        public int CategoryId { get; set; }
        public CampaignCategory Category { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Range(1, int.MaxValue)]
        public int TargetMeals { get; set; }

        public int MealsPrepared { get; set; }

        public int MealsDistributed { get; set; }

        [Required]
        public CampaignStatus Status { get; set; } = CampaignStatus.Draft;

        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;

        public ICollection<Donation>? Donations { get; set; }
        public ICollection<DistributionBatch>? DistributionBatches { get; set; }
    }
}
