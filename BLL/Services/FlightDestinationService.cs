using System.Data;
using AutoMapper;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Mappers;
using DAL.Data;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services;

public class FlightDestinationService
{
    private readonly IFlightDestinationRepository _flightDestinationRepository;
    private readonly IMapper _mapper;

    public FlightDestinationService(IFlightDestinationRepository flightDestinationRepository, IMapper mapper)
    {
        _flightDestinationRepository = flightDestinationRepository;
        _mapper = mapper;
    }

    public async Task UpdateFlightDestination(FlightDestinationDTO flightDestinationDto)
    {
        var flightDestination = _mapper.Map<FlightDestination>(flightDestinationDto);
        
        await _flightDestinationRepository.ReplaceAsync(flightDestination);
    }
    public async Task<int> RegisterFlightDestination(FlightDestinationDTO flightDestinationDto)
    {
        FlightDestination destination = _mapper.Map<FlightDestination>(flightDestinationDto);
        (Passenger, List<FlightDestination>) map =
            await _flightDestinationRepository.GetPassengersWithFlights(flightDestinationDto.PassengerId);

        var currentFlights = _mapper.Map<List<FlightDestinationDTO>>(map.Item2);

        if (!CheckIsPassengerAvailable(currentFlights, flightDestinationDto))
        {
            throw new TooFrequentFlightsException($"Passenger {map.Item1.Id} has flight in close hour.");
        }
        
        int newId = await _flightDestinationRepository.AddAsync(destination);

        return newId;
    }

    public async Task<FlightDestinationDTO> GetFlightDestination(int id)
    {
        FlightDestination flightDestination = await _flightDestinationRepository.GetAsync(id);
     
        return _mapper.Map<FlightDestinationDTO>(flightDestination);
    }
    
    
    public async Task<List<FlightDestinationDTO>> GetAllFlightDestinations()
    {
        var flightDestinationList = await _flightDestinationRepository.GetAllAsync();
     
        return _mapper.Map<List<FlightDestinationDTO>>(flightDestinationList);
    }

    public async Task DeleteFlightDestination(int id)
    {
        await _flightDestinationRepository.DeleteAsync(id);
    }

    private bool CheckIsPassengerAvailable(List<FlightDestinationDTO> currentFlights, FlightDestinationDTO newFLight)
    {
        return currentFlights.Any(flight => Math.Abs((flight.Start - newFLight.Start).Hours) < 2);
    }
        
}
