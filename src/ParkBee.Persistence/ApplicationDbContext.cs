using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ParkBee.Domain.AggregatesModel;
using ParkBee.Domain.SeedWork;
using ParkBee.Persistence.Models;

namespace ParkBee.Persistence
{
    //public class SchoolDBInitializer : DropCreateDatabaseAlways<SchoolDBContext>
    //{
    //    protected override void Seed(ApplicationDbContext context)
    //    {
    //        IList<Garage> defaultStandards = new List<Garage>();

    //        defaultStandards.Add(new Garage() { Name = "Garage 1", Identifier = Guid.NewGuid() });
    //        defaultStandards.Add(new Garage() { Name = "Garage 1", Identifier = Guid.NewGuid() });
    //        defaultStandards.Add(new Garage() { Name = "Garage 1", Identifier = Guid.NewGuid() });

    //        context.Garages.AddRange(defaultStandards);

    //        base.Seed(context);
    //    }
    //}

    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IUnitOfWork
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var garageIdentifier = Guid.NewGuid();
            var garage = new Garage() { Name = $"Garage {garageIdentifier}", Identifier = garageIdentifier };
            builder.Entity<Garage>().HasData(garage);

            var doorIdentifier = Guid.NewGuid();
            builder.Entity<Door>().HasData(new Door { Name = $"Door {doorIdentifier}", Identifier = doorIdentifier, GarageIdentifier = garage.Identifier, IpAddress = "127.0.0.1" });

            base.OnModelCreating(builder);
        }

        public DbSet<Garage> Garages { get; set; }
        public DbSet<Door> Doors { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<DoorStatusHistory> DoorStatusHistory { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await base.SaveChangesAsync(cancellationToken);
            return true;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableDetailedErrors();
            optionsBuilder.LogTo(Console.WriteLine);
            

            base.OnConfiguring(optionsBuilder);
        }
    }
}
