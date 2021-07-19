using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkBee.Domain.AggregatesModel;

namespace ParkBee.Persistence.DbConfiguration
{
    class OwnerDbConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.ToTable("Owners");

            builder.HasKey(door => door.Identifier);
            builder.Property(door => door.Id).ValueGeneratedOnAdd();
            builder.Property(door => door.Identifier).ValueGeneratedOnAdd();
            builder.Property(address => address.GarageIdentifier).IsRequired();

            builder.Property(address => address.FirstName).IsRequired();
            builder.Property(address => address.LastName).IsRequired();
            builder.Property(address => address.Email).IsRequired();
            builder.Property(address => address.Phone).IsRequired();
        }
    }
}
