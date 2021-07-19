using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkBee.Domain.AggregatesModel;

namespace ParkBee.Persistence.DbConfiguration
{
    class DoorStatusHistoryDbConfiguration : IEntityTypeConfiguration<DoorStatusHistory>
    {
        public void Configure(EntityTypeBuilder<DoorStatusHistory> builder)
        {
            builder.ToTable("DoorStatusHistory");

            builder.Property(doorStatusHistory => doorStatusHistory.Id).ValueGeneratedOnAdd();

            builder.Property(doorStatusHistory => doorStatusHistory.DoorIdentifier).IsRequired().ValueGeneratedNever();
            builder.Property(doorStatusHistory => doorStatusHistory.NewStatus).IsRequired().HasMaxLength(64);
            builder.Property(doorStatusHistory => doorStatusHistory.OldStatus).IsRequired().HasMaxLength(40);
            builder.Property(doorStatusHistory => doorStatusHistory.Id).IsRequired();
        }
    }
}
