using Domain.Entities;

namespace Application.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetRolesAsync();
    }
}
