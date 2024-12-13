using Domain.Contact.Entity;
using Domain.MockData.RegionsMock;
using Domain.Region.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Context
{
    public class TechChallengeContext : DbContext
    {
        public TechChallengeContext(DbContextOptions options) : base(options)
        {
        }

        public TechChallengeContext()
        {
        }

        public virtual DbSet<ContactEntity> Contacts { get; set; }
        public virtual DbSet<RegionEntity> Regions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ContactEntity>()
            .HasOne(c => c.Region)
            .WithMany(r => r.Contacts)
            .HasForeignKey(c => c.RegionId);

            modelBuilder.Entity<RegionEntity>().HasData(RegionsMock.GetMockRegions());
        }
    }
}
