using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FEventopia.DAO.EntityModels.Base;
using FEventopia.DAO.EntityModels;

namespace FEventopia.DAO.DbContext
{
    public class FEventopiaDbContext : IdentityDbContext<Account>
    {
        public FEventopiaDbContext() { }

        public FEventopiaDbContext(DbContextOptions<FEventopiaDbContext> options) : base(options) { }

        public DbSet<Account> Account { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<SponsorEvent> SponsorEvent { get; set; }
        public DbSet<EventStall> EventStall { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<EventDetail> EventDetail { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<EntityModels.Task> Task { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<SponsorManagement> sponsorManagement { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name).HasMaxLength(50).HasColumnType("nvarchar(50)");
                entity.Property(x => x.Email).HasMaxLength(50).HasColumnType("nvarchar(50)").IsRequired(false);
                entity.Property(x => x.PhoneNumber).HasMaxLength(10).HasColumnType("varchar(10)").IsRequired(false);
                entity.Property(x => x.Avatar).HasColumnType("nvarchar(MAX)").IsRequired(false);
                entity.Property(x => x.Role).HasColumnType("nvarchar(20)");
                entity.Property(x => x.CreditAmount).HasColumnType("float");
            });

            builder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.TransactionType).HasMaxLength(3).HasColumnType("varchar(3)");
                entity.Property(x => x.Amount).HasColumnType("float");
                entity.Property(x => x.Description).HasColumnType("nvarchar(MAX)");
                entity.Property(x => x.TransactionDate).HasColumnType("datetime");
                entity.Property(x => x.Status).HasColumnType("bit");

                entity.HasOne(t => t.Account).WithMany(a => a.Transaction)
                      .HasForeignKey(t => t.AccountID).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Ticket");
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => x.VisitorID).IsUnique(false);
                entity.HasIndex(x => x.TransactionID).IsUnique(false);
                entity.HasIndex(x => x.EventDetailID).IsUnique(false);

                entity.Property(x => x.CheckInStatus).HasColumnType("bit");

                entity.HasOne(t => t.Account).WithMany(a => a.Ticket)
                      .HasForeignKey(t => t.VisitorID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(t => t.Transaction).WithOne(t => t.Ticket)
                      .HasForeignKey<Ticket>(t => t.TransactionID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(t => t.EventDetail).WithMany(ed => ed.Ticket)
                      .HasForeignKey(t => t.EventDetailID).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<EventStall>(entity =>
            {
                entity.ToTable("EventStall");
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => x.SponsorID).IsUnique(false);
                entity.HasIndex(x => x.TransactionID).IsUnique(false);
                entity.HasIndex(x => x.EventDetailID).IsUnique(false);

                entity.Property(es => es.StallNumber).HasMaxLength(10).HasColumnType("varchar(10)");

                entity.HasOne(es => es.Account).WithMany(a => a.EventStall)
                      .HasForeignKey(es => es.SponsorID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(es => es.Transaction).WithOne(t => t.EventStall)
                      .HasForeignKey<EventStall>(es => es.TransactionID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(es => es.EventDetail).WithMany(ed => ed.EventStall)
                      .HasForeignKey(es => es.EventDetailID).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<SponsorEvent>(entity =>
            {
                entity.ToTable("SponsorEvent");
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => x.SponsorID).IsUnique(false);
                entity.HasIndex(x => x.TransactionID).IsUnique(false);
                entity.HasIndex(x => x.EventID).IsUnique(false);

                entity.HasOne(se => se.Account).WithMany(a => a.SponsorEvents)
                      .HasForeignKey(se => se.SponsorID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(se => se.Transaction).WithOne(t => t.SponsorEvent)
                      .HasForeignKey<SponsorEvent>(se => se.TransactionID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(se => se.Event).WithOne(e => e.SponsorEvent)
                      .HasForeignKey<SponsorEvent>(se => se.EventID).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Event>(entity =>
            {
                entity.ToTable("Event");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.EventName).HasMaxLength(50).HasColumnType("nvarchar(50)");
                entity.Property(x => x.EventDescription).HasColumnType("nvarchar(MAX)");
                entity.Property(x => x.Category).HasMaxLength(10).HasColumnType("nvarchar(20)");
                entity.Property(x => x.Banner).HasColumnType("varchar(MAX)");
                entity.Property(x => x.InitialCapital).HasColumnType("float");
                entity.Property(x => x.SponsorCapital).HasColumnType("float");
                entity.Property(x => x.TicketSaleIncome).HasColumnType("float");
                entity.Property(x => x.StallSaleIncome).HasColumnType("float");
                entity.Property(x => x.Status).HasColumnType("nvarchar(20)");
            });

            builder.Entity<EventDetail>(entity =>
            {
                entity.ToTable("EventDetail");
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => x.LocationID).IsUnique(false);
                entity.HasIndex(x => x.EventID).IsUnique(false);

                entity.Property(x => x.StartDate).HasColumnType("datetime");
                entity.Property(x => x.EndDate).HasColumnType("datetime");
                entity.Property(x => x.TicketForSaleInventory).HasColumnType("int");
                entity.Property(x => x.TicketPrice).HasColumnType("float");
                entity.Property(x => x.EstimateCost).HasColumnType("float");
                entity.Property(x => x.StallPrice).HasColumnType("float");

                entity.HasOne(ed => ed.Event).WithMany(e => e.EventDetail)
                      .HasForeignKey(ed => ed.EventID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(ed => ed.Location).WithMany(l => l.EventDetail)
                      .HasForeignKey(ed => ed.LocationID).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<EntityModels.Task>(entity =>
            {
                entity.ToTable("Task");
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => x.StaffID).IsUnique(false);
                entity.HasIndex(x => x.EventDetailID).IsUnique(false);

                entity.Property(x => x.Description).HasColumnType("nvarchar(MAX)");
                entity.Property(x => x.Status).HasColumnType("nvarchar(20)");
                entity.Property(x => x.PlanCost).HasColumnType("float");
                entity.Property(x => x.ActualCost).HasColumnType("float");

                entity.HasOne(t => t.Account).WithMany(a => a.Task)
                      .HasForeignKey(t => t.StaffID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(t => t.EventDetail).WithMany(e => e.Task)
                      .HasForeignKey(t => t.EventDetailID).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.LocationName).HasMaxLength(50).HasColumnType("nvarchar(50)");
                entity.Property(x => x.LocationDescription).HasColumnType("nvarchar(MAX)");
                entity.Property(x => x.Capacity).HasColumnType("int");
            });

            builder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => x.AccountId).IsUnique(false);
                entity.HasIndex(x => x.EventDetailId).IsUnique(false);

                entity.Property(x => x.Rate).HasColumnType("int");
                entity.Property(x => x.Description).HasColumnType("nvarchar(MAX)");

                entity.HasOne(f => f.Account).WithMany(a => a.Feedback)
                      .HasForeignKey(f => f.AccountId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(f => f.EventDetail).WithMany(ed => ed.Feedbacks)
                      .HasForeignKey(f => f.EventDetailId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<SponsorManagement>(entity =>
            {
                entity.ToTable("SponsorManagement");
                entity.HasKey(p => new { p.EventId, p.SponsorId });

                entity.HasIndex(p => p.EventId).IsUnique(false);
                entity.HasIndex(p => p.SponsorId).IsUnique(false);

                entity.Property(x => x.PledgeAmount).HasColumnType("float");
                entity.Property(x => x.ActualAmount).HasColumnType("float");
                entity.Property(x => x.Status).HasColumnType("nvarchar(20)");
                entity.Property(x => x.Rank).HasColumnType("nvarchar(20)");

                entity.HasOne(sm => sm.Account).WithMany(a => a.SponsorManagement)
                      .HasForeignKey(sm => sm.SponsorId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sm => sm.Event).WithMany(e => e.SponsorManagement)
                      .HasForeignKey(sm => sm.EventId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<EventAssignee>(entity =>
            {
                entity.ToTable("EventAssignee");
                entity.HasKey(p => new { p.AccountId, p.EventDetailId });

                entity.HasIndex(p => p.AccountId).IsUnique(false);
                entity.HasIndex(p => p.EventDetailId).IsUnique(false);

                entity.Property(x => x.Role).HasColumnType("nvarchar(20)");

                entity.HasOne(ea => ea.EventDetail).WithMany(ed => ed.EventAssignee)
                      .HasForeignKey(ea => ea.EventDetailId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(ea => ea.Account).WithMany(a => a.EventAssignee)
                      .HasForeignKey(ea => ea.AccountId).OnDelete(DeleteBehavior.Restrict);
            });
        }

        public override int SaveChanges()
        {
            DateTime utcNow = DateTime.UtcNow;

            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is EntityBase && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((EntityBase)entityEntry.Entity).UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, vietnamTimeZone);

                if (entityEntry.State == EntityState.Added)
                {
                    ((EntityBase)entityEntry.Entity).CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, vietnamTimeZone);
                }
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            DateTime utcNow = DateTime.UtcNow;

            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is EntityBase && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((EntityBase)entityEntry.Entity).UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, vietnamTimeZone);

                if (entityEntry.State == EntityState.Added)
                {
                    ((EntityBase)entityEntry.Entity).CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, vietnamTimeZone);
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
