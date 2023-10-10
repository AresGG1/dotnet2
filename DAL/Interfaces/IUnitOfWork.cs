namespace DAL.Interfaces;

public interface IUnitOfWork : IDisposable
{
    public IAircraftRepository AircraftRepository { get; }
    public IFlightDestinationRepository FlightDestinationRepository { get; }
    public IPassengerRepository PassengerRepository { get; }
    public IAirportRepository AirportRepository { get; }
    void Commit();
    void Dispose();
}
