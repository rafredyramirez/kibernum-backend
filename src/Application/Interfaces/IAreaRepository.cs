using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAreaRepository
    {
        Task<List<Area>> GetAreasAsync();
    }
}
