using System.ComponentModel.DataAnnotations;

namespace BLL.DTO;

public class AircraftDTO
{
    public int Id { get; set; }
    [Required]
    [MaxLength(30)]
    public string Manufacturer { get; set; }
    [Required]
    [MaxLength(30)]
    public string Model { get; set; }
    [Required]
    [Range(1965, 2023)]
    public int Year { get; set; }
    [Required]
    public int FlightHours { get; set; }
}