using AutoMapper;
using ParkBee.Domain.AggregatesModel;
using ParkBee.WebApplication.Shared;

namespace ParkBee.WebApplication.Server.AutoMapper
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<Garage, GarageDto>();
            CreateMap<GarageDto, Garage>();
            CreateMap<Door, DoorDto>().ForMember(dest => dest.DoorStatus, opt => opt.MapFrom(src => src.Status));
            CreateMap<DoorStatus, DoorStatusDto>();
            CreateMap<DoorStatusDto, DoorStatus>();
            CreateMap<DoorDto, Door>();
            CreateMap<AddressDto, Address>();
            CreateMap<Address, AddressDto>();
            CreateMap<Owner, OwnerDto>();
            CreateMap<OwnerDto, Owner>();
        }
    }
}
