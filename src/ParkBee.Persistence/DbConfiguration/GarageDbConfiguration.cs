using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkBee.Domain.AggregatesModel;

namespace ParkBee.Persistence.DbConfiguration
{
    class GarageDbConfiguration : IEntityTypeConfiguration<Garage>
    {
        public void Configure(EntityTypeBuilder<Garage> builder)
        {
            builder.ToTable("Garages");

            builder.HasKey(g => g.Identifier);
            builder.Property(g => g.Id).ValueGeneratedOnAdd();

            builder.Property(g => g.Name).IsRequired().HasMaxLength(64);
            builder.Property(g => g.Identifier).ValueGeneratedOnAdd();

            builder.HasMany(garage => garage.Doors)
                .WithOne(c => c.Garage)
                .HasForeignKey(door => door.GarageIdentifier)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(garage => garage.Owner)
                .WithOne(owner => owner.Garage)
                .HasForeignKey<Owner>(owner => owner.GarageIdentifier)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(garage => garage.Address)
                .WithOne(address => address.Garage)
                .HasForeignKey<Address>(address => address.GarageIdentifier)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
