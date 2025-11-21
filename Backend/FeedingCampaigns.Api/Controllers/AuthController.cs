using System.Security.Cryptography;
using System.Text;
using FeedingCampaigns.Api.Data;
using FeedingCampaigns.Api.Models;
using FeedingCampaigns.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedingCampaigns.Api.Controllers
{
    public record RegisterRequest(
        string FullName,
        string Email,
        string? PhoneNumber,
        string Password,
        UserRole Role,
        int? BranchId);

    public record LoginRequest(string Email, string Password);

    public record AuthResponse(int UserId, string FullName, string Email, string Role, string Token);

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IJwtTokenService _tokenService;

        public AuthController(AppDbContext db, IJwtTokenService tokenService)
        {
            _db = db;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exists = await _db.Users.AnyAsync(u => u.Email == request.Email);
            if (exists)
                return BadRequest("Email already registered.");

            CreatePasswordHash(request.Password, out var hash, out var salt);

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Role = request.Role,
                BranchId = request.BranchId,
                PasswordHash = hash,
                PasswordSalt = salt,
                IsActive = true
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var token = _tokenService.GenerateToken(user);

            return new AuthResponse(user.Id, user.FullName, user.Email, user.Role.ToString(), token);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !user.IsActive)
                return Unauthorized("Invalid credentials.");

            if (!VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
                return Unauthorized("Invalid credentials.");

            var token = _tokenService.GenerateToken(user);

            return new AuthResponse(user.Id, user.FullName, user.Email, user.Role.ToString(), token);
        }

        private static void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA256();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA256(storedSalt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computed.SequenceEqual(storedHash);
        }
    }
}
