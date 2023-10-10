using System.Data.Common;
using DAL.Interfaces;
using DAL.Models;
using MySqlConnector;

namespace DAL.Data;

public class FlightDestinationRepository : GenericRepository<FlightDestination>, IFlightDestinationRepository
{
    public FlightDestinationRepository(MySqlConnection sqlConnection) 
        : base(sqlConnection, "FlightDestinations")
    {
    }
    
    public async Task<(Passenger, List<FlightDestination>)> GetPassengersWithFlights(int id)
    {
        string commandText =    
            string.Format(@"SELECT * FROM {0} JOIN {1} ON {2}.id = " 
                          + @"FlightDestinations.PassengerId WHERE {3}.id = @id",
                "Passengers",
                _tableName,
                "Passengers",
                "Passengers"
            );
            
        var command = _sqlConnection.CreateCommand();
        command.CommandText = commandText;
        
        MySqlParameter typeParameter = new MySqlParameter("@id", id);
        command.Parameters.Add(typeParameter);

        Passenger passenger = new Passenger();
        List<FlightDestination> flightDestinations = new List<FlightDestination>();
        
        using (DbDataReader reader = await command.ExecuteReaderAsync())
        {

            if (!reader.HasRows)
            {
                throw new KeyNotFoundException($"Passengers with id [{id}] could not be found.");
            }
            
            while (await reader.ReadAsync())
            {
                if (passenger.Id == 0)
                {
                    passenger.Id = await reader.GetFieldValueAsync<int>(0);
                    passenger.Name = await reader.GetFieldValueAsync<string>(1);
                    passenger.Surname = await reader.GetFieldValueAsync<string>(2);
                    passenger.Email = await reader.GetFieldValueAsync<string>(3);
                }
                
                FlightDestination flightDestination = new FlightDestination
                {
                    Id = await reader.GetFieldValueAsync<int>(4),
                    AirportId = await reader.GetFieldValueAsync<int>(5),
                    Start = await reader.GetFieldValueAsync<DateTime>(6),
                    PassengerId = await reader.GetFieldValueAsync<int>(7),
                    TicketPrice = await reader.GetFieldValueAsync<decimal>(8),
                };
                
                flightDestinations.Add(flightDestination);
            }
            

        }
        
        return (passenger, flightDestinations);
    }
    
}
