namespace BLL.DTO;

public class FlightDestinationDTO
{
    public int Id { get; set; }
    public int AirportId { get; set; }
    public DateTime Start { get; set; }
    public int AircraftId { get; set; }
    public int PassengerId { get; set; }
    public decimal TicketPrice { get; set; }
}