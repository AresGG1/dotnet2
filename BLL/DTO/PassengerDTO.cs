using System.ComponentModel.DataAnnotations;

namespace BLL.DTO;

public class PassengerDTO
{
    public int Id { get; set; }
    [Required]
    [MaxLength(30)]
    public string Name { get; set; }
    [Required]
    [MaxLength(30)]
    public string Surname { get; set; }
    [Required]
    [MaxLength(30)]
    public string Email { get; set; }
}
