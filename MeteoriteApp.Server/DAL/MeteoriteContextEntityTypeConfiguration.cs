using MeteoriteApp.Server.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeteoriteApp.Server.DAL
{
    public class MeteoriteContextEntityTypeConfiguration : IEntityTypeConfiguration<Meteorite>
    {
        void IEntityTypeConfiguration<Meteorite>.Configure(EntityTypeBuilder<Meteorite> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Mass);
            builder.HasIndex(x => x.Year);
            builder.HasIndex(x => x.RecClass);
        }
    }
}
