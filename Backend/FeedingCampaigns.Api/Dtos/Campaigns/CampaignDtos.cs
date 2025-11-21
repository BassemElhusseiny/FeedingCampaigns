using System.ComponentModel.DataAnnotations;
using FeedingCampaigns.Api.Models;

namespace FeedingCampaigns.Api.Dtos.Campaigns
{
    public class CampaignCreateDto
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required, MaxLength(2000)]
        public string Description { get; set; } = null!;

        [Required]
        public int BranchId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Range(1, int.MaxValue)]
        public int TargetMeals { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }
    }

    public class CampaignSummaryDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string BranchName { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int TargetMeals { get; set; }

        public int MealsPrepared { get; set; }

        public int MealsDistributed { get; set; }

        public string Status { get; set; } = null!;
    }

    public class CampaignDetailsDto : CampaignSummaryDto
    {
        public string Description { get; set; } = null!;

        public string CreatedByName { get; set; } = null!;

        public int TotalDonationsCount { get; set; }

        public decimal TotalMonetaryDonations { get; set; }
    }

    public class CampaignStatusUpdateDto
    {
        [Required]
        public CampaignStatus Status { get; set; }
    }
}
