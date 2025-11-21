using System.ComponentModel.DataAnnotations;
using FeedingCampaigns.Api.Models;

namespace FeedingCampaigns.Api.Dtos.Donations
{
    public class DonationItemCreateDto
    {
        [Required]
        public int FoodItemId { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }

        public string? Unit { get; set; }
    }

    public class DonationCreateDto
    {
        [Required]
        public int CampaignId { get; set; }

        public int? DonorUserId { get; set; }

        [Required]
        public DonationType Type { get; set; }

        public decimal? MonetaryAmount { get; set; }

        public string? PaymentMethod { get; set; }

        public List<DonationItemCreateDto>? Items { get; set; }
    }

    public class DonationListItemDto
    {
        public int Id { get; set; }

        public DateTime DonatedAt { get; set; }

        public string? DonorName { get; set; }

        public string Type { get; set; } = null!;

        public string Status { get; set; } = null!;

        public decimal? MonetaryAmount { get; set; }
    }
}
