using System.ComponentModel.DataAnnotations;

namespace FeedingCampaigns.Api.Models
{
    public class Ngo
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(300)]
        public string? LegalName { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(50)]
        public string? Country { get; set; }

        [MaxLength(30)]
        public string? PhoneNumber { get; set; }

        [MaxLength(200)]
        public string? Email { get; set; }

        public ICollection<Branch>? Branches { get; set; }
    }
}
