using System.ComponentModel.DataAnnotations;

namespace FeedingCampaigns.Api.Models
{
    public class DistributionBatch
    {
        public int Id { get; set; }

        [Required]
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; } = null!;

        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }

        public DateTime DistributionDate { get; set; }

        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;

        public ICollection<DistributionLine>? DistributionLines { get; set; }
    }
}
