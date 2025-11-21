using System.ComponentModel.DataAnnotations;

namespace FeedingCampaigns.Api.Models
{
    public class CampaignCategory
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = null!;

        [MaxLength(300)]
        public string? Description { get; set; }

        public ICollection<Campaign>? Campaigns { get; set; }
    }
}
