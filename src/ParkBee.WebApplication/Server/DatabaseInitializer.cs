using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ParkBee.Domain.AggregatesModel;
using ParkBee.Persistence;

namespace ParkBee.WebApplication.Server
{
    public class DatabaseInitializer
    {
        public static void SeedData(IServiceProvider services)
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            if (context.Garages.Any())
            {
                return;
            }

            var garageIdentifier = Guid.NewGuid();
            var owner = new Owner { Identifier = Guid.NewGuid(), GarageIdentifier = garageIdentifier, FirstName = "Armin", LastName = "van Buuren", Email = "abcd1@gmail.com", Phone = "+31 0686867756" };
            var address = new Address { Identifier = Guid.NewGuid(), GarageIdentifier = garageIdentifier, City = "Amsterdam", Country = "The Netherlands", State = "North Holland", Street = "Oudekerksplein 23", ZipCode = "1012 GX" };

            var garage = new Garage
            {
                Name = "Garage 1",
                Identifier = garageIdentifier,
                Doors =
                {
                    new Door
                    {
                        Name = "Door 1", Identifier = Guid.NewGuid(), GarageIdentifier = garageIdentifier, IpAddress = "127.0.0.1", Status = DoorStatus.Offline
                    },
                    new Door
                    {
                        Name = "Door 2", Identifier = Guid.NewGuid(), GarageIdentifier = garageIdentifier, IpAddress = "127.0.0.2", Status = DoorStatus.Online
                    },
                    new Door
                    {
                        Name = "Door 3", Identifier = Guid.NewGuid(), GarageIdentifier = garageIdentifier, IpAddress = "127.0.0.3", Status = DoorStatus.OnlineAndAlmostFull
                    },
                    new Door
                    {
                        Name = "Door 4", Identifier = Guid.NewGuid(), GarageIdentifier = garageIdentifier, IpAddress = "127.0.0.4", Status = DoorStatus.OnlineAndQuiteEmpty
                    },
                    new Door
                    {
                        Name = "Door 5", Identifier = Guid.NewGuid(), GarageIdentifier = garageIdentifier, IpAddress = "127.0.0.5", Status = DoorStatus.OnlineButFull
                    }
                },
                Owner = owner,
                Address = address
            };

            context.Garages.Add(garage);
            
            context.Owners.Add(owner);
            context.Addresses.Add(address);
            context.SaveChanges();
        }
    }
}