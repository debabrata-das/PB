using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkBee.Domain.AggregatesModel;

namespace ParkBee.Persistence.DbConfiguration
{
    class AddressDbConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(address => address.Identifier);
            builder.Property(address => address.Id).ValueGeneratedOnAdd();
            builder.Property(address => address.Identifier).ValueGeneratedOnAdd();
            builder.Property(address => address.GarageIdentifier).IsRequired();

            builder.Property(address => address.City).IsRequired();
            builder.Property(address => address.Country).IsRequired();
            builder.Property(address => address.State).IsRequired();
            builder.Property(address => address.Street).IsRequired();
            builder.Property(address => address.ZipCode).IsRequired();
        }
    }
}
