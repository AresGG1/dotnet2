using DAL.Models;

namespace DAL.Interfaces;

public interface IAircraftTypeRepository : IGenericRepository<AircraftType>
{
    public Task<IEnumerable<Aircraft>> SelectAircraftsByTypeId(int typeId);
}
