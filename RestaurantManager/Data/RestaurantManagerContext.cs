using Microsoft.EntityFrameworkCore;
using RestaurantManager.Models;

namespace RestaurantManager.Data
{
    public partial class RestaurantManagerContext : DbContext
    {
        public RestaurantManagerContext()
        {
        }

        public RestaurantManagerContext(DbContextOptions<RestaurantManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Adjustment> Adjustments { get; set; } = null!;
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<Food> Foods { get; set; } = null!;
        public virtual DbSet<FoodCategory> FoodCategories { get; set; } = null!;
        public virtual DbSet<MapOrderAdjustment> MapOrdersAdjustments { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<FoodEarnings> FoodEarnings { get; set; }
        public DbSet<FoodQuantities> FoodQuantities { get; set; }
        public DbSet<CategoryEarnings> CategoryEarnings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=MORTEZA\\SQL2019;Initial Catalog=RestaurantManager;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<FoodEarnings>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<FoodQuantities>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<CategoryEarnings>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<Adjustment>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Food>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.PicturePath).HasMaxLength(256);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Foods)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Foods_FoodCategories");
            });

            modelBuilder.Entity<FoodCategory>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<MapOrderAdjustment>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.AdjustmentId });

                entity.ToTable("Map_Orders_Adjustments");

                entity.HasOne(d => d.Adjustment)
                    .WithMany(p => p.MapOrdersAdjustments)
                    .HasForeignKey(d => d.AdjustmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Map_Orders_Adjustments_Adjustments");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.MapOrdersAdjustments)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Map_Orders_Adjustments_Orders");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CustomerAddress).HasMaxLength(256);

                entity.Property(e => e.CustomerMobileNumber)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName).HasMaxLength(100);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasOne(d => d.Food)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.FoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderItems_Food");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_OrderItems_Orders");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}