using System.Text.RegularExpressions;
using AutoMapper;
using BLL.DTO;
using BLL.Exceptions;
using DAL.Data;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services;

public class PassengerService
{
    private readonly IPassengerRepository _passengerRepository;
    private readonly IMapper _mapper;

    public PassengerService(IPassengerRepository passengerRepository, IMapper mapper)
    {
        _passengerRepository = passengerRepository;
        _mapper = mapper;
    }

    public async Task UpdatePassenger(PassengerDTO passengerDto)
    {
        var passengerPersistence = _mapper.Map<Passenger>(passengerDto);
        
        await _passengerRepository.ReplaceAsync(passengerPersistence);
    }
    public async Task DeletePassenger(int id)
    {
        if (await HasActiveFlights(id))
        {
            throw new ActiveFlightException($"Passenger {id} has an active flight");
        }
        
        await _passengerRepository.DeleteAsync(id);
    }

    public async Task<List<PassengerDTO>> GetALlPassengers()
    {
        var passengersList = await _passengerRepository.GetAllAsync();
        var passengersDtoList = _mapper.Map<List<PassengerDTO>>(passengersList);

        return passengersDtoList;
    }

    public async Task<int> RegisterPassenger(PassengerDTO passengerDto)
    {
        if (!IsValidEmail(passengerDto.Email))
        {
            throw new FormatException("Invalid email format.");
        }

        var persistenceModel = _mapper.Map<Passenger>(passengerDto);
        
        int newId = await _passengerRepository.AddAsync(persistenceModel);

        return newId;
    }

    public async Task<PassengerDTO> GetPassenger(int id)
    {
        var passenger = await _passengerRepository.GetAsync(id);
        var passengerDto = _mapper.Map<PassengerDTO>(passenger);

        return passengerDto;
    }

    private bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        return Regex.IsMatch(email, pattern);
    }

    private async Task<bool> HasActiveFlights(int passengerId)
    {
        var destinations = 
            await _passengerRepository.GetFlightDestinationsByPassenger(passengerId);
        
        return destinations.Exists(dest => dest.Start > (new DateTime()));
    }
}
