using FeedingCampaigns.Api.Data;
using FeedingCampaigns.Api.Dtos.Distributions;
using FeedingCampaigns.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedingCampaigns.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DistributionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DistributionsController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/distributions
        [HttpPost]
        [Authorize(Roles = "BranchManager,NgoAdmin,SystemAdmin")]
        public async Task<ActionResult> CreateBatch([FromBody] DistributionBatchCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var campaign = await _context.Campaigns.FindAsync(dto.CampaignId);
            if (campaign == null)
                return BadRequest("Campaign not found.");

            var creator = await _context.Users.FindAsync(dto.CreatedByUserId);
            if (creator == null)
                return BadRequest("Creator user not found.");

            var batch = new DistributionBatch
            {
                CampaignId = dto.CampaignId,
                BranchId = dto.BranchId,
                DistributionDate = dto.DistributionDate,
                CreatedByUserId = dto.CreatedByUserId
            };

            batch.DistributionLines = new List<DistributionLine>();

            int totalMeals = 0;

            foreach (var lineDto in dto.Lines)
            {
                var family = await _context.BeneficiaryFamilies.FindAsync(lineDto.BeneficiaryFamilyId);
                if (family == null)
                    return BadRequest($"BeneficiaryFamily with id {lineDto.BeneficiaryFamilyId} not found.");

                var line = new DistributionLine
                {
                    BeneficiaryFamilyId = lineDto.BeneficiaryFamilyId,
                    MealsDelivered = lineDto.MealsDelivered
                };
                batch.DistributionLines.Add(line);
                totalMeals += lineDto.MealsDelivered;
            }

            _context.DistributionBatches.Add(batch);

            campaign.MealsDistributed += totalMeals;

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBatchesForCampaign), new { campaignId = dto.CampaignId }, new { batch.Id });
        }

        // GET: api/distributions/by-campaign/5
        [HttpGet("by-campaign/{campaignId:int}")]
        public async Task<ActionResult<IEnumerable<DistributionBatchSummaryDto>>> GetBatchesForCampaign(int campaignId)
        {
            var exists = await _context.Campaigns.AnyAsync(c => c.Id == campaignId);
            if (!exists)
                return NotFound("Campaign not found.");

            var batches = await _context.DistributionBatches
                .Include(b => b.DistributionLines!)
                .Where(b => b.CampaignId == campaignId)
                .AsNoTracking()
                .OrderByDescending(b => b.DistributionDate)
                .Select(b => new DistributionBatchSummaryDto
                {
                    Id = b.Id,
                    CampaignId = b.CampaignId,
                    DistributionDate = b.DistributionDate,
                    TotalMealsDelivered = b.DistributionLines!.Sum(l => l.MealsDelivered),
                    FamiliesServed = b.DistributionLines!.Count
                })
                .ToListAsync();

            return Ok(batches);
        }
    }
}
