using System.Data.Common;
using DAL.Interfaces;
using DAL.Models;
using MySqlConnector;

namespace DAL.Data;

public class AircraftTypeRepository : GenericRepository<AircraftType>, IAircraftTypeRepository
{
    public AircraftTypeRepository(MySqlConnection sqlConnection)
        : base(sqlConnection, "AircraftTypes")
    {
    }

    public async Task<IEnumerable<Aircraft>> SelectAircraftsByTypeId(int typeId)
    {
        string commandText = @"SELECT * FROM aircrafts WHERE typeId = @TypeId";
        
        var command = _sqlConnection.CreateCommand();
        command.CommandText = commandText;
        
        MySqlParameter typeParameter = new MySqlParameter("@TypeId", typeId);
        command.Parameters.Add(typeParameter);

        List<Aircraft> list = new List<Aircraft>();
        
        using (DbDataReader reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                Aircraft aircraft = new Aircraft
                {
                    Id = await reader.GetFieldValueAsync<int>(0),
                    Manufacturer = await reader.GetFieldValueAsync<string>(1),
                    Model = await reader.GetFieldValueAsync<string>(2),
                    Year = await reader.GetFieldValueAsync<int>(3),
                    FlightHours = await reader.GetFieldValueAsync<int>(4),
                    // TypeId = await reader.GetFieldValueAsync<int>(5),
                };
                
                list.Add(aircraft);
            }
        }

        return list;
    }
    
    public async Task<AircraftType> GetAsync(int id)
    {
        string commandText =    
            string.Format(@"SELECT * FROM {0} JOIN Aircrafts ON {1}.id = Aircrafts.typeId WHERE {2}.id = @id",
                _tableName,
                _tableName,
                _tableName
            );
            
        var command = _sqlConnection.CreateCommand();
        command.CommandText = commandText;
        
        MySqlParameter typeParameter = new MySqlParameter("@id", id);
        command.Parameters.Add(typeParameter);

        AircraftType type = new AircraftType();
        
        using (DbDataReader reader = await command.ExecuteReaderAsync())
        {

            if (!reader.HasRows)
            {
                throw new KeyNotFoundException($"{_tableName} with id [{id}] could not be found.");
            }

            List<Aircraft> aircraftList = new List<Aircraft>();
            
            while (await reader.ReadAsync())
            {
                if (type.Id == 0)
                {
                    type.Id = await reader.GetFieldValueAsync<int>(0);
                    type.TypeName = await reader.GetFieldValueAsync<string>(1);
                }
                
                Aircraft aircraft = new Aircraft
                {
                    Id = await reader.GetFieldValueAsync<int>(2),
                    Manufacturer = await reader.GetFieldValueAsync<string>(3),
                    Model = await reader.GetFieldValueAsync<string>(4),
                    Year = await reader.GetFieldValueAsync<int>(5),
                    FlightHours = await reader.GetFieldValueAsync<int>(6),
                    // TypeId = await reader.GetFieldValueAsync<int>(7),
                };
                
                aircraftList.Add(aircraft);
            }
            type.Aircrafts = aircraftList;

        }
        
        return type;
    }
    
}
