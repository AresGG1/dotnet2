using System.ComponentModel;
using System.Reflection;
using System.Text;
using DAL.Interfaces;
using Dapper;
using MySqlConnector;

namespace DAL.Data;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
{

    protected MySqlConnection _sqlConnection;
    
    protected readonly string _tableName;

    protected GenericRepository(MySqlConnection sqlConnection, string tableName)
    {
        _sqlConnection = sqlConnection;
        _tableName = tableName;
    }


    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _sqlConnection.QueryAsync<T>($"SELECT * FROM {_tableName}");
    }

    public async Task<T> GetAsync(int id)
    {
        var result = await _sqlConnection.QuerySingleOrDefaultAsync<T>(
            $"SELECT * FROM {_tableName} WHERE Id=@Id", 
            param: new { Id = id });
        
        if (result == null)
            throw new KeyNotFoundException($"{_tableName} with id [{id}] could not be found.");
        
        return result;
    }

    public async Task DeleteAsync(int id)
    {
        await _sqlConnection.ExecuteAsync($"DELETE FROM {_tableName} WHERE Id=@Id", param: new { Id = id });
    }

    public async Task<int> AddAsync(T t)
    {
        var insertQuery = GenerateInsertQuery();
        var newId = await _sqlConnection.ExecuteScalarAsync<int>(insertQuery, param: t);
        
        return newId;
    }

    public async Task<int> AddRangeAsync(IEnumerable<T> list)
    {
        var inserted = 0;
        var query = GenerateInsertQuery();
        inserted += await _sqlConnection.ExecuteAsync(query, 
            param: list);
        
        return inserted;
    }


    public async Task ReplaceAsync(T t)
    {
        var updateQuery = GenerateUpdateQuery();
        await _sqlConnection.ExecuteAsync(updateQuery, param: t);
    }
    
    private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();
    private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
    {
        return (from prop in listOfProperties
                let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                select prop.Name).ToList();
    }
    
    private string GenerateUpdateQuery()
    {
        var updateQuery = new StringBuilder($"UPDATE {_tableName} SET ");
        var properties = GenerateListOfProperties(GetProperties);
        properties.ForEach(property =>
        {
            if (!property.Equals("Id"))
            {
                updateQuery.Append($"{property}=@{property},");
            }
        });
        updateQuery.Remove(updateQuery.Length - 1, 1);
        updateQuery.Append(" WHERE Id=@Id");
        
        return updateQuery.ToString();
    }
    
    private string GenerateInsertQuery()
    {
        var insertQuery = new StringBuilder($"INSERT INTO {_tableName} ");
        insertQuery.Append("(");
        var properties = GenerateListOfProperties(GetProperties);

        properties.Remove("Id");
        properties.ForEach(prop => { insertQuery.Append($"{prop},"); });
        insertQuery
            .Remove(insertQuery.Length - 1, 1)
            .Append(") VALUES (");

        properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });
        insertQuery
            .Remove(insertQuery.Length - 1, 1)
            .Append(")");
        insertQuery.Append("; SELECT LAST_INSERT_ID()");

        return insertQuery.ToString();
    }
}
