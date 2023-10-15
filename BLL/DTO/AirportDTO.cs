using System.ComponentModel.DataAnnotations;

namespace BLL.DTO;

public class AirportDTO
{
    public int Id { get; set; }
    [Required]
    [MaxLength(30)]
    public string AirportName { get; set; }
    [Required]
    [MaxLength(30)]
    public string Country { get; set; }
}
