using Go_Cart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Go_Cart.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users", "security");
            builder.Entity<IdentityRole>().ToTable("Roles", "security");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "security");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "security");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "security");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "security");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "security");
            
            builder.Entity<ProductSize>()
                .HasKey(ps => new {ps.SizeId, ps.ProductId});

            builder.Entity<ProductColor>()
                .HasKey(pc => new { pc.ColorId, pc.ProductId });

            builder.Entity<OrderItem>()
                .HasKey(po => new { po.OrderId, po.ProductId });

            builder.Entity<WishListItem>()
                .HasKey(wi => new { wi.WishListId, wi.ProductId });
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<WishListItem> WishListItems { get; set; }

    }
}