using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(string name, string contact, int roleId);
        Task UpdateUserAsync(int userId, string name, string contact, int roleId);
        Task AssignAreaAsync(int userId, int areaId);
        Task DeleteUserAsync(int userId);
        Task<List<UserGridDto>> GetLastUsersAsync();       
    }
}
