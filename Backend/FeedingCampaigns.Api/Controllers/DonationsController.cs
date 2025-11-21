using FeedingCampaigns.Api.Data;
using FeedingCampaigns.Api.Dtos.Donations;
using FeedingCampaigns.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedingCampaigns.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DonationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DonationsController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/donations
        [HttpPost]
        public async Task<ActionResult> CreateDonation([FromBody] DonationCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var campaign = await _context.Campaigns.FindAsync(dto.CampaignId);
            if (campaign == null)
                return BadRequest("Campaign not found.");

            if (dto.DonorUserId.HasValue)
            {
                var donor = await _context.Users.FindAsync(dto.DonorUserId.Value);
                if (donor == null)
                    return BadRequest("Donor user not found.");
            }

            if (dto.Type == DonationType.Money && !dto.MonetaryAmount.HasValue)
                return BadRequest("MonetaryAmount is required for money donations.");

            var donation = new Donation
            {
                CampaignId = dto.CampaignId,
                DonorUserId = dto.DonorUserId,
                Type = dto.Type,
                MonetaryAmount = dto.MonetaryAmount,
                PaymentMethod = dto.PaymentMethod,
                Status = DonationStatus.Approved
            };

            if (dto.Items != null && dto.Items.Any())
            {
                donation.Items = new List<DonationItem>();
                foreach (var itemDto in dto.Items)
                {
                    var foodItem = await _context.FoodItems.FindAsync(itemDto.FoodItemId);
                    if (foodItem == null)
                        return BadRequest($"Food item with id {itemDto.FoodItemId} not found.");

                    var di = new DonationItem
                    {
                        FoodItemId = itemDto.FoodItemId,
                        Quantity = itemDto.Quantity,
                        Unit = itemDto.Unit ?? foodItem.Unit
                    };
                    donation.Items.Add(di);

                    var inv = new InventoryTransaction
                    {
                        CampaignId = dto.CampaignId,
                        FoodItemId = itemDto.FoodItemId,
                        Type = InventoryTransactionType.InboundFromDonation,
                        QuantityDelta = itemDto.Quantity,
                        Donation = donation
                    };
                    _context.InventoryTransactions.Add(inv);
                }
            }

            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDonation), new { id = donation.Id }, new { donation.Id });
        }

        // GET: api/donations/by-campaign/5
        [HttpGet("by-campaign/{campaignId:int}")]
        public async Task<ActionResult<IEnumerable<DonationListItemDto>>> GetDonationsForCampaign(int campaignId)
        {
            var exists = await _context.Campaigns.AnyAsync(c => c.Id == campaignId);
            if (!exists)
                return NotFound("Campaign not found.");

            var donations = await _context.Donations
                .Include(d => d.DonorUser)
                .Where(d => d.CampaignId == campaignId)
                .AsNoTracking()
                .OrderByDescending(d => d.DonatedAt)
                .Select(d => new DonationListItemDto
                {
                    Id = d.Id,
                    DonatedAt = d.DonatedAt,
                    DonorName = d.DonorUser != null ? d.DonorUser.FullName : "Anonymous",
                    Type = d.Type.ToString(),
                    Status = d.Status.ToString(),
                    MonetaryAmount = d.MonetaryAmount
                })
                .ToListAsync();

            return Ok(donations);
        }

        // GET: api/donations/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetDonation(int id)
        {
            var donation = await _context.Donations
                .Include(d => d.Campaign)
                .Include(d => d.DonorUser)
                .Include(d => d.Items!)
                    .ThenInclude(i => i.FoodItem)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donation == null)
                return NotFound();

            return Ok(donation);
        }
    }
}
