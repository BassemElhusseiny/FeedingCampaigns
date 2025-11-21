using System.ComponentModel.DataAnnotations;

namespace FeedingCampaigns.Api.Dtos.Beneficiaries
{
    public class BeneficiaryCreateDto
    {
        [Required, MaxLength(50)]
        public string FamilyCode { get; set; } = null!;

        [Required, MaxLength(150)]
        public string HeadOfFamilyName { get; set; } = null!;

        public string? NationalId { get; set; }

        public string? Address { get; set; }

        public string? Area { get; set; }

        public string? PhoneNumber { get; set; }

        public int MembersCount { get; set; }

        public int VulnerabilityScore { get; set; }

        public int? BranchId { get; set; }
    }

    public class BeneficiaryListItemDto
    {
        public int Id { get; set; }

        public string FamilyCode { get; set; } = null!;

        public string HeadOfFamilyName { get; set; } = null!;

        public string? Area { get; set; }

        public string? PhoneNumber { get; set; }

        public int MembersCount { get; set; }

        public int VulnerabilityScore { get; set; }
    }
}
