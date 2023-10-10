using DAL.Models;

namespace DAL.Interfaces;

public interface IFlightDestinationRepository : IGenericRepository<FlightDestination>
{
    Task<(Passenger, List<FlightDestination>)> GetPassengersWithFlights(int id);
}
