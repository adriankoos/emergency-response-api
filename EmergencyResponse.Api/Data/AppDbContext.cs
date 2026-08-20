using Microsoft.EntityFrameworkCore;
using EmergencyResponse.Api.Models;

namespace EmergencyResponse.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<IncidentUnit> IncidentUnits { get; set; }
    }
}