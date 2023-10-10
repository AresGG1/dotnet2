namespace DAL.Models;

public class AircraftType
{
    public int Id {get; set; }
    public string TypeName { get; set; }
    public List<Aircraft> Aircrafts { get; set; }
}
