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
        public DbSet<Cost> Cost { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<EventDetail> EventDetail { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<EntityModels.Task> Task { get; set; }

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
                entity.Property(x => x.Avatar).HasColumnType("nvarchar(MAX)");
                entity.Property(x => x.Role).HasColumnType("nvarchar(10)");
                entity.Property(x => x.CreditAmount).HasColumnType("float");
            });

            builder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.TransactionType).HasMaxLength(3).HasColumnType("varchar(3)");
                entity.Property(x => x.Amount).HasColumnType("float");
                entity.Property(x => x.Description).HasColumnType("nvarchar(MAX)");
                entity.Property(x => x.TransactionDate).HasColumnType("date");
                entity.Property(x => x.Status).HasColumnType("bit");

                entity.HasOne(t => t.Account).WithMany(a => a.Transaction)
                      .HasForeignKey(t => t.AccountID).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Ticket");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.CheckInStatus).HasColumnType("bit");

                entity.HasOne(t => t.Account).WithMany(a => a.Ticket)
                      .HasForeignKey(t => t.VisitorID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(t => t.Transaction).WithOne(t => t.Ticket)
                      .HasForeignKey<Ticket>(t => t.TransactionID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(t => t.EventDetail).WithOne(ed => ed.Ticket)
                      .HasForeignKey<Ticket>(t => t.EventDetailID).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<EventStall>(entity =>
            {
                entity.ToTable("EventStall");
                entity.HasKey(x => x.Id);

                entity.Property(es => es.StallNumber).HasMaxLength(10).HasColumnType("varchar(10)");

                entity.HasOne(es => es.Account).WithMany(a => a.EventStall)
                      .HasForeignKey(es => es.SponsorID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(es => es.Transaction).WithOne(t => t.EventStall)
                      .HasForeignKey<EventStall>(es => es.TransactionID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(es => es.EventDetail).WithOne(ed => ed.EventStall)
                      .HasForeignKey<EventStall>(es => es.EventDetailID).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<SponsorEvent>(entity =>
            {
                entity.ToTable("SponsorEvent");
                entity.HasKey(x => x.Id);

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
                entity.Property(x => x.Category).HasMaxLength(10).HasColumnType("nvarchar(10)");
                entity.Property(x => x.Banner).HasColumnType("varchar(MAX)");

                entity.HasOne(e => e.Account).WithMany(a => a.Event)
                      .HasForeignKey(e => e.OperatorID).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<EventDetail>(entity =>
            {
                entity.ToTable("EventDetail");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.EventDate).HasColumnType("date");
                entity.Property(x => x.EventTime).HasColumnType("time");
                entity.Property(x => x.TicketForSaleInventory).HasColumnType("int");
                entity.Property(x => x.TicketPrice).HasColumnType("float");
                entity.Property(x => x.EstimateCost).HasColumnType("float");

                entity.HasOne(ed => ed.Event).WithMany(e => e.EventDetail)
                      .HasForeignKey(ed => ed.EventID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(ed => ed.Location).WithOne(l => l.EventDetail)
                      .HasForeignKey<EventDetail>(ed => ed.LocationID).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<EntityModels.Task>(entity =>
            {
                entity.ToTable("Task");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Description).HasColumnType("nvarchar(MAX)");
                entity.Property(x => x.Status).HasColumnType("bit");

                entity.HasOne(t => t.Account).WithMany(a => a.Task)
                      .HasForeignKey(t => t.StaffID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(t => t.EventDetail).WithMany(e => e.Task)
                      .HasForeignKey(t => t.EventDetailID).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Cost>(entity =>
            {
                entity.ToTable("Cost");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.CostPurpose).HasColumnType("nvarchar(MAX)");
                entity.Property(x => x.CostAmount).HasColumnType("float");

                entity.HasOne(c => c.EventDetail).WithMany(e => e.Cost)
                      .HasForeignKey(c => c.EventDetailID).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.LocationName).HasMaxLength(50).HasColumnType("nvarchar(50)");
                entity.Property(x => x.LocationDescription).HasColumnType("nvarchar(MAX)");

                entity.HasOne(l => l.EventDetail).WithOne(ed => ed.Location)
                      .HasForeignKey<EventDetail>(ed => ed.LocationID).OnDelete(DeleteBehavior.Restrict);
            });
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is EntityBase && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((EntityBase)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((EntityBase)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is EntityBase && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((EntityBase)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((EntityBase)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
