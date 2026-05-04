
using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task CreateUserAsync(string name, string contact, int roleId, string performedBy);
        Task UpdateUserAsync(int userId, string name, string contact, int roleId, string performedBy);
        Task AssignAreaAsync(int userId, int areaId, string performedBy);
        Task DeleteUserAsync(int userId, string performedBy);
        Task<List<UserGridDto>> GetLastUsersAsync();
    }
}
