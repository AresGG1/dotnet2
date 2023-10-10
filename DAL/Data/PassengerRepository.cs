using System.Data.Common;
using DAL.Interfaces;
using DAL.Models;
using MySqlConnector;

namespace DAL.Data;

public class PassengerRepository : GenericRepository<Passenger>, IPassengerRepository
{
    public PassengerRepository(MySqlConnection sqlConnection) 
        : base(sqlConnection, "Passengers")
    {
    }
    
    public async Task<List<FlightDestination>> GetFlightDestinationsByPassenger(int passengerId)
    {
        string commandText = @$"SELECT * FROM {_tableName} WHERE passengerId = @passengerId";
        
        var command = _sqlConnection.CreateCommand();
        command.CommandText = commandText;
        
        MySqlParameter typeParameter = new MySqlParameter("@passengerId", passengerId);
        command.Parameters.Add(typeParameter);

        List<FlightDestination> list = new List<FlightDestination>();
        
        using (DbDataReader reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                FlightDestination flightDestination = new FlightDestination
                {
                    Id = await reader.GetFieldValueAsync<int>(0),
                    AirportId = await reader.GetFieldValueAsync<int>(1),
                    Start = await reader.GetFieldValueAsync<DateTime>(2),
                    AircraftId = await reader.GetFieldValueAsync<int>(3),
                    PassengerId = await reader.GetFieldValueAsync<int>(4),
                    TicketPrice = await reader.GetFieldValueAsync<decimal>(5),
                    
                };
                
                list.Add(flightDestination);
            }
        }

        return list;
    }
    
}
