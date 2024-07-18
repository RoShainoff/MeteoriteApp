using MeteoriteApp.Server.DAL.Models;
using Microsoft.EntityFrameworkCore;

public class MeteoriteContext : DbContext
{
    public MeteoriteContext(DbContextOptions<MeteoriteContext> options) : base(options) { }

    public DbSet<Meteorite> Meteorites { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}