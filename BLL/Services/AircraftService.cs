using AutoMapper;
using BLL.DTO;
using DAL.Data;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services;

public class AircraftService
{
    private readonly IAircraftRepository _aircraftRepository;
    private readonly IMapper _mapper;

    public AircraftService(IAircraftRepository aircraftRepository, IMapper mapper)
    {
        _aircraftRepository = aircraftRepository;
        _mapper = mapper;
    }

    public async Task DeleteAircraft(int id)
    {
        await _aircraftRepository.DeleteAsync(id);
    }

    public async Task UpdateAircraft(AircraftDTO aircraftDto)
    {
        Aircraft aircraftPersistence = _mapper.Map<Aircraft>(aircraftDto);

        await _aircraftRepository.ReplaceAsync(aircraftPersistence);
    }

    public async Task<int> RegisterAircraft(AircraftDTO aircraftDto)
    {
        Aircraft aircraft = _mapper.Map<Aircraft>(aircraftDto);

        int newId = await _aircraftRepository.AddAsync(aircraft);
        
        return newId;
    }
    
    public async Task<AircraftDTO> GetAircraft(int id)
    {
        Aircraft aircraft = await _aircraftRepository.GetAsync(id);
        
        return _mapper.Map<AircraftDTO>(aircraft);
    }
    
    public async Task<List<AircraftDTO>> GetAllAircrafts()
    {
        var aircraftList = await _aircraftRepository.GetAllAsync();
        
        return _mapper.Map<List<AircraftDTO>>(aircraftList);
    }
    
}
