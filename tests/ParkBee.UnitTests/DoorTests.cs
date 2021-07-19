using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using ParkBee.WebApplication.Server;
using ParkBee.WebApplication.Server.Commands;
using ParkBee.WebApplication.Server.Controllers;
using ParkBee.WebApplication.Server.Exceptions;
using ParkBee.WebApplication.Shared;

namespace ParkBee.UnitTests
{
    [TestFixture]
    public class DoorTests : TestBase
    {
        [Test]
        public async Task SaveDoorToRepository_ShouldAddDoorWithSameDetails()
        {
            var guid = Guid.NewGuid();
            var guidDoor = Guid.NewGuid();

            var door = await SaveDoorToRepository(guid, guidDoor);
            
            Assert.IsNotNull(door);
            Assert.That(door.Identifier, Is.EqualTo(guidDoor));
            Assert.That(door.Name, Is.EqualTo(TestDoorName));
            Assert.That(door.GarageIdentifier, Is.EqualTo(guid));
        }

        [Test]
        public async Task GetDoorByIdentifier_ShouldGetDoorWithSameDetails()
        {
            var guidGarage = Guid.NewGuid();
            var guidDoor = Guid.NewGuid();
            await SaveDoorToRepository(guidGarage, guidDoor);

            DoorsController doorsController =
                new DoorsController(GetDoorRepository(), new LoggerManager(), GetMapper(), null);

            var actionResult = await doorsController.GetDoorByIdentifier(guidDoor);

            var doorDto = ((DoorDto)((OkObjectResult) actionResult.Result).Value);
            Assert.That(doorDto.Identifier, Is.EqualTo(guidDoor));
            Assert.That(doorDto.GarageIdentifier, Is.EqualTo(guidGarage));
            Assert.That(doorDto.Name, Is.EqualTo("Test Door 1"));
        }

        [Test]
        public async Task RemoveDoor_ShouldRemoveDoorWithSameIdentifier()
        {
            var guid = Guid.NewGuid();
            var guidDoor = Guid.NewGuid();
            await SaveDoorToRepository(guid, guidDoor);
            DoorsController doorsController =
                new DoorsController(GetDoorRepository(), new LoggerManager(), GetMapper(), null);

            await doorsController.RemoveDoor(guidDoor);
            var actionResult = await doorsController.GetDoorByIdentifier(guidDoor);
            Assert.That(actionResult.Result, Is.InstanceOf(typeof(NotFoundResult)));
        }

        [Test]
        public void AddNewDoorWithInvalidData_ShouldThrowDataValidationException()
        {
            var guid = Guid.NewGuid();
            var saveDoorHandler= new SaveDoorCommandHandler(GetDoorRepository(), GetGarageRepository(), GetMapper(), new LoggerManager());

            DataValidationException ex = Assert.ThrowsAsync<DataValidationException>(async () =>
            {
                await saveDoorHandler.Handle(new SaveDoorCommand() { Identifier = guid, Name = "" }, new CancellationToken(true));
            });

            Assert.That(ex?.Message, Is.EqualTo("Door validation failed"));
            Assert.That(ex?.InnerException, Is.InstanceOf(typeof(AggregateException)));
            Assert.That(ex?.InnerException?.InnerException?.Message, Is.EqualTo("The name can not be null or empty"));
        }

        [Test]
        public void AddNewDoorWithInvalidGarage_ShouldThrowDataValidationException()
        {
            var invalidGarageIdentifier = Guid.NewGuid();
            var guid = Guid.NewGuid();
            var saveDoorHandler = new SaveDoorCommandHandler(GetDoorRepository(), GetGarageRepository(), GetMapper(), new LoggerManager());

            DataValidationException ex = Assert.ThrowsAsync<DataValidationException>(async () =>
            {
                await saveDoorHandler.Handle(new SaveDoorCommand() { Identifier = guid, Name = TestDoorName, IpAddress = TestDoorIpAddress,  GarageIdentifier = invalidGarageIdentifier}, new CancellationToken(true));
            });

            Assert.That(ex?.Message, Is.EqualTo($"Cannot create Door since the Garage with Identifier '{invalidGarageIdentifier}' not found"));
        }

        [Test]
        public async Task Handle_UpdateDoorDetails_ShouldGetDoorWithSameDetails()
        {
            var guidGarage = Guid.NewGuid();
            var guidDoor = Guid.NewGuid();
            await SaveDoorToRepository(guidGarage, guidDoor);
            var saveDoorHandler = new SaveDoorCommandHandler(GetDoorRepository(), GetGarageRepository(), GetMapper(), new LoggerManager());

            var doorNewName = $"{TestDoorName}-{Guid.NewGuid()}";
            var doorOutput = await saveDoorHandler.Handle(new SaveDoorCommand() { Identifier = guidGarage, Name = doorNewName, IpAddress = TestDoorIpAddress, GarageIdentifier = guidGarage }, new CancellationToken(true));

            Assert.IsNotNull(doorOutput);
            Assert.That(doorOutput.Name, Is.EqualTo(doorNewName));
        }

        [Test]
        public async Task AddDuplicateDoor_ShouldThrowCannotAddDuplicateEntityException()
        {
            var guidGarage = Guid.NewGuid();
            SaveGarageCommand saveGarageCommand = new SaveGarageCommand { Identifier = guidGarage, Name = TestGarageName };
            await SaveGarageToRepository(saveGarageCommand);

            var saveDoorHandler = new SaveDoorCommandHandler(GetDoorRepository(), GetGarageRepository(), GetMapper(), new LoggerManager());
            var guid = Guid.NewGuid();
            var saveDoor = new SaveDoorCommand() { Identifier = guid, Name = TestDoorName, IpAddress = TestDoorIpAddress, GarageIdentifier = guidGarage };

            await saveDoorHandler.Handle(saveDoor, new CancellationToken(true));

            CannotAddDuplicateEntityException exc = Assert.ThrowsAsync<CannotAddDuplicateEntityException>(async () =>
            {
                await saveDoorHandler.Handle(saveDoor, new CancellationToken(true));
            });

            Assert.IsNotNull(exc);
            Assert.That(exc.Message, Is.EqualTo($"Cannot add duplicate Door with {guid}"));
        }

        [Test]
        public void GetByIdentifier_ShouldReturnFromRepositoryWithSameDetails()
        {
            var doorRepository = GetDoorRepository();
            var guid = Guid.NewGuid();

            ExecuteSync(async () =>
            {
                await doorRepository.Add(GetDemoDoor(TestDoorName, guid, GetDemGarage(TestDoorName, guid), TestDoorIpAddress));
                //await doorRepository.UnitOfWork.SaveChangesAsync(new CancellationToken(true));
            });

            var sut = ExecuteSync(() => doorRepository.GetByIdentifier(guid));

            Assert.IsNotNull(sut);
            Assert.That(sut.Identifier, Is.EqualTo(guid));
            Assert.That(sut.Name, Is.EqualTo(TestDoorName));
            Assert.That(sut.GarageIdentifier, Is.EqualTo(guid));
        }
    }
}
