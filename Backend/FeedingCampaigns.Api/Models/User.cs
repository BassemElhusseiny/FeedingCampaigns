using System.ComponentModel.DataAnnotations;

namespace FeedingCampaigns.Api.Models
{
    public enum UserRole
    {
        Donor = 1,
        Volunteer = 2,
        BranchManager = 3,
        NgoAdmin = 4,
        SystemAdmin = 5
    }

    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string FullName { get; set; } = null!;

        [Required, MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [MaxLength(30)]
        public string? PhoneNumber { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; } = null!;

        [Required]
        public byte[] PasswordSalt { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }

        public ICollection<Campaign>? CreatedCampaigns { get; set; }
        public ICollection<Donation>? Donations { get; set; }
    }
}
