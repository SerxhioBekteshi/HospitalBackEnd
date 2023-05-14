using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;

namespace Repository;

public class RepositoryContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public RepositoryContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new TimeZoneConfiguration());
    }

    public DbSet<EmailTemplate>? EmailTemplate { get; set; }
    public DbSet<ApplicationMenu>? ApplicationMenu { get; set; }
    public DbSet<Services>? Services { get; set; }
    public DbSet<Device>? Devices { get; set; }
    public DbSet<ServiceDevice>? ServiceDevice { get; set; }
    public DbSet<ServiceStaff>? ServiceStaff { get; set; }
    public DbSet<WorkingHourService>? WorkingHourService { get; set; }
    public DbSet<Reservation>? Reservations { get; set; }
    public DbSet<Reports>? Reports { get; set; }
    public DbSet<Delay>? Delays { get; set; }


}