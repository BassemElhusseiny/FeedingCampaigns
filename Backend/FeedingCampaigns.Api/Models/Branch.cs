using System.ComponentModel.DataAnnotations;

namespace FeedingCampaigns.Api.Models
{
    public class Branch
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = null!;

        [Required]
        public int NgoId { get; set; }
        public Ngo Ngo { get; set; } = null!;

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(30)]
        public string? PhoneNumber { get; set; }

        public ICollection<User>? Users { get; set; }
        public ICollection<Campaign>? Campaigns { get; set; }
    }
}
