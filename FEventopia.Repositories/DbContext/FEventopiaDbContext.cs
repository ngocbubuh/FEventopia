using FEventopia.Repositories.EntityModels;
using FEventopia.Repositories.EntityModels.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FEventopia.Repositories.DbContext
{
    public class FEventopiaDbContext : IdentityDbContext<Account>
    {
        public FEventopiaDbContext() { }

        public FEventopiaDbContext(DbContextOptions<FEventopiaDbContext> options) : base(options) { }

        public DbSet<Account> Account { get; set; }

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
                //The rest for Identity to generate
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
                ((EntityBase)entityEntry.Entity).UpdatedBy = "";

                if (entityEntry.State == EntityState.Added)
                {
                    ((EntityBase)entityEntry.Entity).CreatedDate = DateTime.Now;
                    ((EntityBase)entityEntry.Entity).CreatedBy = "";
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
                ((EntityBase)entityEntry.Entity).UpdatedBy = "";

                if (entityEntry.State == EntityState.Added)
                {
                    ((EntityBase)entityEntry.Entity).CreatedDate = DateTime.Now;
                    ((EntityBase)entityEntry.Entity).CreatedBy = "";
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
