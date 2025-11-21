using FeedingCampaigns.Api.Data;
using FeedingCampaigns.Api.Dtos.Campaigns;
using FeedingCampaigns.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedingCampaigns.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CampaignsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CampaignsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/campaigns
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampaignSummaryDto>>> GetCampaigns()
        {
            var campaigns = await _context.Campaigns
                .Include(c => c.Branch)
                .Include(c => c.Category)
                .AsNoTracking()
                .OrderByDescending(c => c.StartDate)
                .Select(c => new CampaignSummaryDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    BranchName = c.Branch.Name,
                    CategoryName = c.Category.Name,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    TargetMeals = c.TargetMeals,
                    MealsPrepared = c.MealsPrepared,
                    MealsDistributed = c.MealsDistributed,
                    Status = c.Status.ToString()
                })
                .ToListAsync();

            return Ok(campaigns);
        }

        // GET: api/campaigns/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CampaignDetailsDto>> GetCampaign(int id)
        {
            var campaign = await _context.Campaigns
                .Include(c => c.Branch)
                .Include(c => c.Category)
                .Include(c => c.CreatedByUser)
                .Include(c => c.Donations!)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (campaign == null)
                return NotFound();

            var totalMonetary = campaign.Donations!
                .Where(d => d.MonetaryAmount.HasValue)
                .Sum(d => d.MonetaryAmount!.Value);

            var dto = new CampaignDetailsDto
            {
                Id = campaign.Id,
                Title = campaign.Title,
                BranchName = campaign.Branch.Name,
                CategoryName = campaign.Category.Name,
                StartDate = campaign.StartDate,
                EndDate = campaign.EndDate,
                TargetMeals = campaign.TargetMeals,
                MealsPrepared = campaign.MealsPrepared,
                MealsDistributed = campaign.MealsDistributed,
                Status = campaign.Status.ToString(),
                Description = campaign.Description,
                CreatedByName = campaign.CreatedByUser.FullName,
                TotalDonationsCount = campaign.Donations!.Count,
                TotalMonetaryDonations = totalMonetary
            };

            return Ok(dto);
        }

        // POST: api/campaigns
        [HttpPost]
        [Authorize(Roles = "BranchManager,NgoAdmin,SystemAdmin")]
        public async Task<ActionResult<CampaignDetailsDto>> CreateCampaign([FromBody] CampaignCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var branch = await _context.Branches.FindAsync(dto.BranchId);
            if (branch == null)
                return BadRequest("Branch not found.");

            var category = await _context.CampaignCategories.FindAsync(dto.CategoryId);
            if (category == null)
                return BadRequest("Category not found.");

            var creator = await _context.Users.FindAsync(dto.CreatedByUserId);
            if (creator == null)
                return BadRequest("Creator user not found.");

            var campaign = new Campaign
            {
                Title = dto.Title,
                Description = dto.Description,
                BranchId = dto.BranchId,
                CategoryId = dto.CategoryId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                TargetMeals = dto.TargetMeals,
                MealsPrepared = 0,
                MealsDistributed = 0,
                Status = CampaignStatus.Planned,
                CreatedByUserId = dto.CreatedByUserId
            };

            _context.Campaigns.Add(campaign);
            await _context.SaveChangesAsync();

            var result = new CampaignDetailsDto
            {
                Id = campaign.Id,
                Title = campaign.Title,
                BranchName = branch.Name,
                CategoryName = category.Name,
                StartDate = campaign.StartDate,
                EndDate = campaign.EndDate,
                TargetMeals = campaign.TargetMeals,
                MealsPrepared = campaign.MealsPrepared,
                MealsDistributed = campaign.MealsDistributed,
                Status = campaign.Status.ToString(),
                Description = campaign.Description,
                CreatedByName = creator.FullName,
                TotalDonationsCount = 0,
                TotalMonetaryDonations = 0
            };

            return CreatedAtAction(nameof(GetCampaign), new { id = campaign.Id }, result);
        }

        // PUT: api/campaigns/5/status
        [HttpPut("{id:int}/status")]
        [Authorize(Roles = "BranchManager,NgoAdmin,SystemAdmin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] CampaignStatusUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var campaign = await _context.Campaigns.FindAsync(id);
            if (campaign == null)
                return NotFound();

            campaign.Status = dto.Status;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
