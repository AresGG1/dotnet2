using DAL.Interfaces;
using DAL.Models;
using MySqlConnector;

namespace DAL.Data;

public class AirportRepository : GenericRepository<Airport>, IAirportRepository
{
    public AirportRepository(MySqlConnection sqlConnection) 
        : base(sqlConnection, "Airports")
    {
    }
    
}
