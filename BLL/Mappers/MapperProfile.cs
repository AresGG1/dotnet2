using AutoMapper;
using BLL.DTO;
using DAL.Models;

namespace BLL.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<FlightDestination, FlightDestinationDTO>().ReverseMap();
        CreateMap<Passenger, PassengerDTO>().ReverseMap();
        CreateMap<Airport, AirportDTO>().ReverseMap();
        CreateMap<Aircraft, AircraftDTO>().ReverseMap();
    }
}
