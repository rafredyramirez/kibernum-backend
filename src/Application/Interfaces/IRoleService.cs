using Domain.Entities;

namespace Application.Interfaces
{
    public interface IRoleService
    {
        Task<List<Role>> GetRolesAsync();
    }
}
