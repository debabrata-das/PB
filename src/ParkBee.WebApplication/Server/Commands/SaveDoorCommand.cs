using MediatR;
using ParkBee.WebApplication.Shared;

namespace ParkBee.WebApplication.Server.Commands
{
    public class SaveDoorCommand : DoorDto, IRequest<DoorDto> { }
}