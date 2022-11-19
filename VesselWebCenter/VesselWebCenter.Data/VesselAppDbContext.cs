using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VesselWebCenter.Data.Models;
using VesselWebCenter.Data.Models.Accounts;

namespace VesselWebCenter.Data
{
    public class VesselAppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {        
        public VesselAppDbContext(DbContextOptions<VesselAppDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CrewMember>(e => 
            { 
                e.Property(v=>v.VesselId)
                .IsRequired(false);
            });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ManningCompany> ManningCompanies { get; set; }
        public DbSet<Vessel> Vessels { get; set; }
        public DbSet<CrewMember> CrewMembers { get; set; }
        public DbSet<PortOfCall> PortsOfCall { get; set; }
        public DbSet<DestinationPort> DestinationPorts { get; set; }
        
      
    }
}
