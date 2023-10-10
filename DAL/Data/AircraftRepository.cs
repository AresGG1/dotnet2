using DAL.Interfaces;
using DAL.Models;
using MySqlConnector;

namespace DAL.Data;

public class AircraftRepository : GenericRepository<Aircraft>, IAircraftRepository
{
    public AircraftRepository(MySqlConnection sqlConnection) 
        : base(sqlConnection, "Aircrafts")
    {
    }
}