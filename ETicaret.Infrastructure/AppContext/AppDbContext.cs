using ETicaret.Domain.Entities;
using ETicaret.Infrastructure.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;

namespace ETicaret.Infrastructure.AppContext
{
    public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {

        public const string DevConnectionString = "AppConnectionDev";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
              
        }
         //asdasdsssdasdasd
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<SizeType> SizeTypes { get; set; }
        public virtual DbSet<CategorySizeType> CategorySizeTypes { get; set; }
        public virtual DbSet<CategorySizeTypeProduct> CategorySizeTypeProducts { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<Campaign> Campaigns { get; set; }
        public virtual DbSet<Campaign> CustomerAddresses { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<ProductCampaignLog> ProductCampaignLogs { get; set; }
        public virtual DbSet<Deneme> Denemes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
            builder.ApplyConfigurationsFromAssembly(typeof(IEntityConfiguration).Assembly);
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            SetBaseProperties();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetBaseProperties();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetBaseProperties()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var userId = /*_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? */"UserNotFound";
            foreach (var entry in entries)
            {
                SetIfAdded(entry, userId);
                SetIfModified(entry, userId);
                SetIfDeleted(entry, userId);
            }
        }

        private void SetIfDeleted(EntityEntry<BaseEntity> entry, string userId)
        {
            if (entry.State != EntityState.Deleted)
            {
                return;
            }
            if (entry.Entity is not AuditableEntity entity)
            {
                return;
            }
            entry.Entity.Status = Domain.Enums.Status.Deleted;
            entry.State = EntityState.Modified;
            entity.DeletedDate = DateTime.Now;
            entity.DeletedBy = userId;
        }

        private void SetIfModified(EntityEntry<BaseEntity> entry, string userId)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.Status = Domain.Enums.Status.Updated;
                entry.Entity.UpdatedBy = userId;
                entry.Entity.UpdatedDate = DateTime.Now;
            }
        }

        private void SetIfAdded(EntityEntry<BaseEntity> entry, string userId)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.Status = Domain.Enums.Status.Added;
                entry.Entity.CreatedBy = userId;
                entry.Entity.CreatedDate = DateTime.Now;
            }
        }
    }
}
