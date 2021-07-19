using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using NUnit.Framework;
using ParkBee.Domain.AggregatesModel;
using ParkBee.WebApplication.Server.AutoMapper;
using ParkBee.WebApplication.Server.Commands;
using ParkBee.WebApplication.Server.Exceptions;
using ParkBee.WebApplication.Server.Queries;
using ParkBee.WebApplication.Shared;
using LoggerManager = ParkBee.WebApplication.Server.LoggerManager;

namespace ParkBee.UnitTests
{
    [TestFixture]
    public class GarageTests : TestBase
    {
        [Test]
        public async Task Handle_AddNewGarage_ShouldAddNewGarageToRepository()
        {
            var guid = Guid.NewGuid();
            SaveGarageCommandHandler saveGarageCommandHandler = new SaveGarageCommandHandler(GetGarageRepository(), GetMapper(), new LoggerManager());

            var garageDto = await saveGarageCommandHandler.Handle(new SaveGarageCommand { Identifier = guid, Name = TestGarageName }, new CancellationToken(true));

            Assert.IsNotNull(garageDto);
            Assert.That(garageDto.Identifier, Is.EqualTo(guid));
            Assert.That(garageDto.Name, Is.EqualTo(TestGarageName));
        }


        [Test]
        public void Handle_AddNewGarageWithInvalidData_ShouldThrowDataValidationException()
        {
            var guid = Guid.NewGuid();
            var saveGarageHandler = new SaveGarageCommandHandler(GetGarageRepository(), GetMapper(), new LoggerManager());

            DataValidationException ex = Assert.ThrowsAsync<DataValidationException>(async () =>
            {
                await saveGarageHandler.Handle(new SaveGarageCommand { Identifier = guid, Name = string.Empty }, new CancellationToken(true));
            });

            var ae = (AggregateException)ex?.InnerException;
            ae?.Flatten();

            Assert.That(ex?.Message, Is.EqualTo("Garage validation failed"));
            Assert.That(ex?.InnerException, Is.InstanceOf(typeof(AggregateException)));
            Assert.That(ae?.Message, Does.Contain("The Name value can not be null or empty"));
        }

        [Test]
        public async Task GetGarageByIdentifier_ShouldGetGarageWithSameDetails()
        {
            var guid = Guid.NewGuid();
            var testGarageName = $"Test Garage {Guid.NewGuid()}";
            SaveGarageCommandHandler saveGarageCommandHandler = new SaveGarageCommandHandler(GetGarageRepository(), GetMapper(), new LoggerManager());
            await saveGarageCommandHandler.Handle(new SaveGarageCommand { Identifier = guid, Name = testGarageName }, new CancellationToken(true));

            GetGarageDetailsQueryHandler garageDetailsQueryHandler = new GetGarageDetailsQueryHandler(GetDoorRepository(), GetGarageRepository(), GetMapper(), new LoggerManager());
            var garageDto = await garageDetailsQueryHandler.Handle(new () {Identifier = guid},  new CancellationToken(true));
            
            Assert.That(garageDto.Identifier, Is.EqualTo(guid));
            Assert.That(garageDto.Name, Is.EqualTo(testGarageName));
        }

        [Test]
        public async Task GetByIdentifier_ShouldReturnGarageWithSameDetails()
        {
            var garageRepository = GetGarageRepository();

            var guid = Guid.NewGuid();

            await garageRepository.AddGarage(GetDemGarage(TestGarageName, guid), new CancellationToken(true));
            await garageRepository.UnitOfWork.SaveChangesAsync(new CancellationToken(true));

            Garage sut = await garageRepository.GetByIdentifier(guid, new CancellationToken(true));

            Assert.IsNotNull(sut);
            Assert.That(sut.Name, Is.EqualTo(TestGarageName));
            Assert.That(sut.Identifier, Is.EqualTo(guid));
        }

        [Test]
        public void Map_MapGarageToGarageDTO_ShouldReturnGarageDToWithSameDetails()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<DtoMappingProfile>());
            var mapper = config.CreateMapper();

            var guid = Guid.NewGuid();
            Garage source = new Garage { Identifier = guid, Name = TestGarageName };
            GarageDto garageDto = mapper.Map<GarageDto>(source);

            Assert.That(garageDto.Name, Is.EqualTo(source.Name));
            Assert.That(garageDto.Identifier, Is.EqualTo(source.Identifier));
        }
    }
}
