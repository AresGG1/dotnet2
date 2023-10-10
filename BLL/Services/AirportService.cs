using AutoMapper;
using BLL.DTO;
using DAL.Data;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services;

public class AirportService
{
    private readonly IAirportRepository _airportRepository;
    private readonly IMapper _mapper;

    public AirportService(IAirportRepository airportRepository, IMapper mapper)
    {
        _airportRepository = airportRepository;
        _mapper = mapper;
    }

    public async Task<int> RegisterAirport(AirportDTO airportDto)
    {
        Airport airportPersistence = _mapper.Map<Airport>(airportDto);
        
        int newId = await _airportRepository.AddAsync(airportPersistence);

        return newId;
    }

    public async Task UpdateAirport(AirportDTO airportDto)
    {
        Airport airportPersistence = _mapper.Map<Airport>(airportDto);
        
        await _airportRepository.ReplaceAsync(airportPersistence);
    }

    public async Task DeleteAirport(int id)
    {
        await _airportRepository.DeleteAsync(id);
    }
    public async Task<AirportDTO> GetAirport(int id)
    {
        Airport airport = await _airportRepository.GetAsync(id);
        
        return _mapper.Map<AirportDTO>(airport);
    }
    
    public async Task<List<AirportDTO>> GetAirports()
    {
        IEnumerable<Airport> airportsList = await _airportRepository.GetAllAsync();

        return _mapper.Map<List<AirportDTO>>(airportsList);
    }

}