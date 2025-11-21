using System.Security.Cryptography;
using System.Text;
using FeedingCampaigns.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedingCampaigns.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ngo> Ngos => Set<Ngo>();
        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<User> Users => Set<User>();
        public DbSet<CampaignCategory> CampaignCategories => Set<CampaignCategory>();
        public DbSet<Campaign> Campaigns => Set<Campaign>();
        public DbSet<FoodItem> FoodItems => Set<FoodItem>();
        public DbSet<Donation> Donations => Set<Donation>();
        public DbSet<DonationItem> DonationItems => Set<DonationItem>();
        public DbSet<BeneficiaryFamily> BeneficiaryFamilies => Set<BeneficiaryFamily>();
        public DbSet<DistributionBatch> DistributionBatches => Set<DistributionBatch>();
        public DbSet<DistributionLine> DistributionLines => Set<DistributionLine>();
        public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<BeneficiaryFamily>()
                .HasIndex(b => b.FamilyCode)
                .IsUnique();

            modelBuilder.Entity<Campaign>()
                .HasOne(c => c.CreatedByUser)
                .WithMany(u => u.CreatedCampaigns!)
                .HasForeignKey(c => c.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DonationItem>()
                .HasOne(di => di.Donation)
                .WithMany(d => d.Items!)
                .HasForeignKey(di => di.DonationId);

            modelBuilder.Entity<DistributionLine>()
                .HasOne(l => l.BeneficiaryFamily)
                .WithMany(f => f.DistributionLines!)
                .HasForeignKey(l => l.BeneficiaryFamilyId);
        }
    }

    public class DbInitializer
    {
        private readonly AppDbContext _db;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(AppDbContext db, ILogger<DbInitializer> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            await _db.Database.EnsureCreatedAsync();

            if (!_db.Ngos.Any())
            {
                var ngo = new Ngo
                {
                    Name = "Hope Feeding Foundation",
                    LegalName = "Hope Feeding NGO",
                    City = "Ismailia",
                    Country = "Egypt",
                    PhoneNumber = "01000000000",
                    Email = "info@hopefeeding.org"
                };

                _db.Ngos.Add(ngo);
                await _db.SaveChangesAsync();

                var mainBranch = new Branch
                {
                    Name = "Ismailia Main Branch",
                    NgoId = ngo.Id,
                    City = "Ismailia",
                    Address = "Main street, Ismailia",
                    PhoneNumber = "01000000001"
                };

                _db.Branches.Add(mainBranch);
                await _db.SaveChangesAsync();

                CreatePasswordHash("Admin@123", out var adminHash, out var adminSalt);
                CreatePasswordHash("Manager@123", out var managerHash, out var managerSalt);
                CreatePasswordHash("Donor@123", out var donorHash, out var donorSalt);

                var admin = new User
                {
                    FullName = "System Admin",
                    Email = "admin@hopefeeding.org",
                    PhoneNumber = "01000000002",
                    Role = UserRole.SystemAdmin,
                    BranchId = mainBranch.Id,
                    PasswordHash = adminHash,
                    PasswordSalt = adminSalt,
                    IsActive = true
                };

                var branchManager = new User
                {
                    FullName = "Branch Manager",
                    Email = "manager@hopefeeding.org",
                    PhoneNumber = "01000000003",
                    Role = UserRole.BranchManager,
                    BranchId = mainBranch.Id,
                    PasswordHash = managerHash,
                    PasswordSalt = managerSalt,
                    IsActive = true
                };

                var donor = new User
                {
                    FullName = "Default Donor",
                    Email = "donor@hopefeeding.org",
                    PhoneNumber = "01000000004",
                    Role = UserRole.Donor,
                    BranchId = mainBranch.Id,
                    PasswordHash = donorHash,
                    PasswordSalt = donorSalt,
                    IsActive = true
                };

                _db.Users.AddRange(admin, branchManager, donor);
                await _db.SaveChangesAsync();

                var catRamadan = new CampaignCategory
                {
                    Name = "Ramadan",
                    Description = "Food packages and iftar meals during Ramadan."
                };
                var catWinter = new CampaignCategory
                {
                    Name = "Winter",
                    Description = "Support for families during winter season."
                };

                _db.CampaignCategories.AddRange(catRamadan, catWinter);
                await _db.SaveChangesAsync();

                var rice = new FoodItem
                {
                    Name = "Rice",
                    Unit = "kg",
                    CaloriesPerUnit = 3600,
                    IsPerishable = false
                };
                var oil = new FoodItem
                {
                    Name = "Cooking Oil",
                    Unit = "liter",
                    CaloriesPerUnit = 8000,
                    IsPerishable = false
                };

                _db.FoodItems.AddRange(rice, oil);
                await _db.SaveChangesAsync();

                var family1 = new BeneficiaryFamily
                {
                    FamilyCode = "FAM-001",
                    HeadOfFamilyName = "Ahmed Ali",
                    Address = "Village 1, Ismailia",
                    Area = "Rural",
                    PhoneNumber = "01000000005",
                    MembersCount = 5,
                    VulnerabilityScore = 80,
                    BranchId = mainBranch.Id
                };

                _db.BeneficiaryFamilies.Add(family1);
                await _db.SaveChangesAsync();

                var campaign = new Campaign
                {
                    Title = "Ramadan 2026 Iftar Campaign",
                    Description = "Daily Iftar meals for vulnerable families in Ismailia.",
                    BranchId = mainBranch.Id,
                    CategoryId = catRamadan.Id,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(30),
                    TargetMeals = 5000,
                    MealsPrepared = 0,
                    MealsDistributed = 0,
                    Status = CampaignStatus.Active,
                    CreatedByUserId = branchManager.Id
                };

                _db.Campaigns.Add(campaign);
                await _db.SaveChangesAsync();

                _logger.LogInformation("Database seeded with initial NGO, branch, users, categories, food items, family, and campaign.");
            }
        }

        private static void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA256();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
