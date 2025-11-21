using System.ComponentModel.DataAnnotations;

namespace FeedingCampaigns.Api.Models
{
    public enum DonationType
    {
        FoodItems = 1,
        Money = 2,
        Mixed = 3
    }

    public enum DonationStatus
    {
        Pending = 0,
        Reviewed = 1,
        Approved = 2,
        Rejected = 3
    }

    public class Donation
    {
        public int Id { get; set; }

        [Required]
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; } = null!;

        public int? DonorUserId { get; set; }
        public User? DonorUser { get; set; }

        [Required]
        public DonationType Type { get; set; }

        public decimal? MonetaryAmount { get; set; }

        [MaxLength(50)]
        public string? PaymentMethod { get; set; }

        [Required]
        public DonationStatus Status { get; set; } = DonationStatus.Pending;

        public DateTime DonatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<DonationItem>? Items { get; set; }
    }
}
