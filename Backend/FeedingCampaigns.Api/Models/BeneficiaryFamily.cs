using System.ComponentModel.DataAnnotations;

namespace FeedingCampaigns.Api.Models
{
    public class BeneficiaryFamily
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string FamilyCode { get; set; } = null!;

        [Required, MaxLength(150)]
        public string HeadOfFamilyName { get; set; } = null!;

        [MaxLength(20)]
        public string? NationalId { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? Area { get; set; }

        [MaxLength(30)]
        public string? PhoneNumber { get; set; }

        public int MembersCount { get; set; }

        public int VulnerabilityScore { get; set; }

        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }

        public ICollection<DistributionLine>? DistributionLines { get; set; }
    }
}
