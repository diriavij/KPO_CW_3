using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Domain;
using MassTransit;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<InboxState> InboxStates { get; set; } = null!;
        public DbSet<OutboxState> OutboxStates { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>(b =>
            {
                b.ToTable("Accounts");
                b.HasKey(a => a.Id);

                b.Property(a => a.UserId).IsRequired();
                b.Property(a => a.CreatedAt)
                    .HasColumnType("timestamp with time zone")
                    .IsRequired();

                b.OwnsOne(a => a.Balance, m =>
                {
                    m.Property(p => p.Value)
                        .HasColumnName("Balance")
                        .HasPrecision(18, 2);
                });
            });

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }
}