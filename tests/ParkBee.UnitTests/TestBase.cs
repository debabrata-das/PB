using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ParkBee.Domain.AggregatesModel;
using ParkBee.Persistence;
using ParkBee.Persistence.Repository;
using ParkBee.WebApplication.Server;
using ParkBee.WebApplication.Server.AutoMapper;
using ParkBee.WebApplication.Server.Commands;
using ParkBee.WebApplication.Shared;

namespace ParkBee.UnitTests
{
    public abstract class TestBase
    {
        private static readonly TaskFactory MyTaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);
        public const string TestGarageName = "Test Garage 1";
        public const string TestDoorName = "Test Door 1";
        public const string TestDoorIpAddress = "Test Door IpAddress";
        public DbContextOptions<ApplicationDbContext> ContextOptions;

        protected TestBase()
        {
            ContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "UnitTestParkBee").Options;
            using (var context = new ApplicationDbContext(ContextOptions, GetOperationalStoreOptions()))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        public async Task<GarageDto> SaveGarageToRepository(SaveGarageCommand saveGarageCommand)
        {
            var saveGarageHandler = new SaveGarageCommandHandler(GetGarageRepository(), GetMapper(), new LoggerManager());
            return await saveGarageHandler.Handle(saveGarageCommand, new CancellationToken(true));
        }

        public async Task<DoorDto> SaveDoorToRepository(Guid guidGarage, Guid guidDoor, string doorName = TestDoorName, string ipAddress = TestDoorIpAddress)
        {
            SaveGarageCommand saveGarageCommand = new SaveGarageCommand { Identifier = guidGarage, Name = TestGarageName };
            await SaveGarageToRepository(saveGarageCommand);

            SaveDoorCommand saveDoorCommand = new SaveDoorCommand { Identifier = guidDoor, GarageIdentifier = guidGarage, Name = doorName, IpAddress = ipAddress };
            var doorDto = await SaveDoorToRepository(saveDoorCommand);
            return doorDto;
        }

        public async Task<DoorDto> SaveDoorToRepository(SaveDoorCommand saveDoorCommand)
        {
            var saveDoorHandler = new SaveDoorCommandHandler(GetDoorRepository(), GetGarageRepository(), GetMapper(), new LoggerManager());
            return await saveDoorHandler.Handle(saveDoorCommand, new CancellationToken(true));
        }

        public static TResult ExecuteSync<TResult>(Func<Task<TResult>> func)
        {
            return MyTaskFactory
                .StartNew<Task<TResult>>(func)
                .Unwrap<TResult>()
                .GetAwaiter()
                .GetResult();
        }

        public static void ExecuteSync(Func<Task> func)
        {
            MyTaskFactory
                .StartNew<Task>(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }

        public Door GetDemoDoor(string name, Guid guid, Garage garage, string ipAddress)
        {
            return new Door() { Name = name, Identifier = guid, Garage = garage, IpAddress = ipAddress };
        }

        public Garage GetDemGarage(string name, Guid guid)
        {
            return new Garage() { Name = name, Identifier = guid};
        }

        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<DtoMappingProfile>());
            var mapper = config.CreateMapper();
            return mapper;
        }

        public ApplicationDbContext GetUnitOfWork()
        {
            var context = new ApplicationDbContext(ContextOptions, GetOperationalStoreOptions());
            return context;
        }
        
        public IDoorRepository GetDoorRepository()
        {
            return new DoorRepository(GetUnitOfWork());
        }

        public IGarageRepository GetGarageRepository()
        {
            return new GarageRepository(GetUnitOfWork());
        }

        private static IOptions<OperationalStoreOptions> GetOperationalStoreOptions()
        {
            return Options.Create(new OperationalStoreOptions { DeviceFlowCodes = new TableConfiguration("DeviceCodes"), PersistedGrants = new TableConfiguration("PersistedGrants") });
        }
    }
}
