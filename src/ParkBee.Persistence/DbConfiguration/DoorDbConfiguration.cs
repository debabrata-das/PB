using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkBee.Domain.AggregatesModel;

namespace ParkBee.Persistence.DbConfiguration
{
    class DoorDbConfiguration : IEntityTypeConfiguration<Door>
    {
        public void Configure(EntityTypeBuilder<Door> builder)
        {
            builder.ToTable("Doors");

            builder.HasKey(door => door.Identifier);
            builder.Property(door => door.Id).ValueGeneratedOnAdd();
            builder.Property(door => door.Identifier).ValueGeneratedOnAdd();
            builder.Property(address => address.GarageIdentifier).IsRequired();

            builder.Property(door => door.Name).IsRequired().HasMaxLength(64);
            builder.Property(door => door.IpAddress).IsRequired().HasMaxLength(40);
            builder.Property(door => door.Status).IsRequired().HasConversion<string>();
        }
    }
}
