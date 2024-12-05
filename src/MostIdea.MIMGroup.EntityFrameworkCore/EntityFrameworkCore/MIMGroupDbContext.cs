using MostIdea.MIMGroup.B2B;
using Abp.IdentityServer4vNext;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MostIdea.MIMGroup.Authorization.Delegation;
using MostIdea.MIMGroup.Authorization.Roles;
using MostIdea.MIMGroup.Authorization.Users;
using MostIdea.MIMGroup.Chat;
using MostIdea.MIMGroup.Editions;
using MostIdea.MIMGroup.Friendships;
using MostIdea.MIMGroup.MultiTenancy;
using MostIdea.MIMGroup.MultiTenancy.Accounting;
using MostIdea.MIMGroup.MultiTenancy.Payments;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.EntityFrameworkCore
{
    public class MIMGroupDbContext : AbpZeroDbContext<Tenant, Role, User, MIMGroupDbContext>, IAbpPersistedGrantDbContext
    {
        public virtual DbSet<SalesConsultant> SalesConsultants { get; set; }

        public virtual DbSet<AssistanceVsUser> AssistanceVsUsers { get; set; }

        public virtual DbSet<DynamicEnumItem> DynamicEnumItems { get; set; }

        public virtual DbSet<DynamicEnum> DynamicEnums { get; set; }

        public virtual DbSet<OrderComment> OrderComments { get; set; }

        public virtual DbSet<OrderItem> OrderItems { get; set; }

        public virtual DbSet<WarehouseVsCourier> WarehouseVsCouriers { get; set; }

        public virtual DbSet<Warehouse> Warehouses { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<ProductPricesForHospital> ProductPricesForHospitals { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<ProductCategory> ProductCategories { get; set; }

        public virtual DbSet<Brand> Brands { get; set; }

        public virtual DbSet<AddressInformation> AddressInformations { get; set; }

        public virtual DbSet<HospitalVsUser> HospitalVsUsers { get; set; }

        public virtual DbSet<Hospital> Hospitals { get; set; }

        public virtual DbSet<HospitalGroup> HospitalGroups { get; set; }

        public virtual DbSet<TaxRate> TaxRates { get; set; }

        public virtual DbSet<District> Districts { get; set; }

        public virtual DbSet<City> Cities { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<DealerVsUser> DealerVsUsers { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public MIMGroupDbContext(DbContextOptions<MIMGroupDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}