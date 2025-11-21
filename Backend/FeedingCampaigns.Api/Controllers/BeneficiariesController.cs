using FeedingCampaigns.Api.Data;
using FeedingCampaigns.Api.Dtos.Beneficiaries;
using FeedingCampaigns.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedingCampaigns.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BeneficiariesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BeneficiariesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/beneficiaries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BeneficiaryListItemDto>>> GetFamilies()
        {
            var families = await _context.BeneficiaryFamilies
                .AsNoTracking()
                .OrderByDescending(f => f.VulnerabilityScore)
                .Select(f => new BeneficiaryListItemDto
                {
                    Id = f.Id,
                    FamilyCode = f.FamilyCode,
                    HeadOfFamilyName = f.HeadOfFamilyName,
                    Area = f.Area,
                    PhoneNumber = f.PhoneNumber,
                    MembersCount = f.MembersCount,
                    VulnerabilityScore = f.VulnerabilityScore
                })
                .ToListAsync();

            return Ok(families);
        }

        // POST: api/beneficiaries
        [HttpPost]
        [Authorize(Roles = "BranchManager,NgoAdmin,SystemAdmin")]
        public async Task<ActionResult> CreateFamily([FromBody] BeneficiaryCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exists = await _context.BeneficiaryFamilies.AnyAsync(f => f.FamilyCode == dto.FamilyCode);
            if (exists)
                return BadRequest("FamilyCode already exists.");

            var family = new BeneficiaryFamily
            {
                FamilyCode = dto.FamilyCode,
                HeadOfFamilyName = dto.HeadOfFamilyName,
                NationalId = dto.NationalId,
                Address = dto.Address,
                Area = dto.Area,
                PhoneNumber = dto.PhoneNumber,
                MembersCount = dto.MembersCount,
                VulnerabilityScore = dto.VulnerabilityScore,
                BranchId = dto.BranchId
            };

            _context.BeneficiaryFamilies.Add(family);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFamilies), new { id = family.Id }, new { family.Id });
        }
    }
}
