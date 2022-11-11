using Microsoft.EntityFrameworkCore;
using REDChallenge.Domain.Entities;

namespace REDChallenge.Persistance
{
    public class REDChallengeContext : DbContext
    {
        public REDChallengeContext(DbContextOptions<REDChallengeContext> options): base(options)
        {
        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<OrderType> OrderType { get; set; }
        public DbSet<Order> Order { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasDefaultValueSql("NEWID()");
                entity.Property(e => e.Name).IsRequired()
                    .HasMaxLength(128);
                entity.Property(e => e.IsDeleted).IsRequired()
                    .HasDefaultValue(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasDefaultValueSql("NEWID()");
                entity.Property(e => e.Name).IsRequired()
                    .HasMaxLength(128);
                entity.Property(e => e.IsDeleted).IsRequired()
                    .HasDefaultValue(false);
            });

            modelBuilder.Entity<OrderType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasQueryFilter(e => !e.IsDeleted);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasDefaultValueSql("NEWID()");
                entity.Property(e => e.IsDeleted).IsRequired()
                    .HasDefaultValue(false);
                entity.Property(e => e.CreatedDate).IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(e => e.CustomerId)
                    .IsRequired();

                entity.HasOne(e => e.CreatedBy)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(e => e.CreatedById)
                    .IsRequired();

                entity.HasOne(e => e.OrderType)
                    .WithMany()
                    .HasForeignKey(e => e.OrderTypeId)
                    .IsRequired();
            });

            #region Seed
            modelBuilder.Entity<OrderType>().HasData(
                new OrderType { Id = 1, Name = "Standard" },
                new OrderType { Id = 2, Name = "Sales" },
                new OrderType { Id = 3, Name = "Purchase" },
                new OrderType { Id = 4, Name = "Transfer" },
                new OrderType { Id = 5, Name = "Return" }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = Guid.NewGuid(), Name = "Test user 1" }
            );

            modelBuilder.Entity<Customer>().HasData(
                new User { Id = Guid.NewGuid(), Name = "Test customer 1" }
            );
            #endregion
        }
    }
}