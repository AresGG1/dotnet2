using DAL.Models;

namespace DAL.Interfaces;

public interface IPassengerRepository : IGenericRepository<Passenger>
{
    public Task<List<FlightDestination>> GetFlightDestinationsByPassenger(int passengerId);
}

