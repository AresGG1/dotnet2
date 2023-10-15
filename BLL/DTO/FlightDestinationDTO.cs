using System.ComponentModel.DataAnnotations;

namespace BLL.DTO;

public class FlightDestinationDTO
{
    public int Id { get; set; }
    [Required]
    public int AirportId { get; set; }
    [Required]
    public DateTime Start { get; set; }
    [Required]
    public int AircraftId { get; set; }
    [Required]
    public int PassengerId { get; set; }
    [Required]
    public decimal TicketPrice { get; set; }
}