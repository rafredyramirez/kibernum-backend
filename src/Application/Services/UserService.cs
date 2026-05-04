using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task CreateUserAsync(string name, string contact, int roleId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("El nombre es obligatorio");

            var users = await _repository.GetLastUsersAsync();

            if (users.Any(u => u.Name == name && u.ContactInfo == contact))
                throw new Exception("El usuario ya existe");

            await _repository.CreateUserAsync(name, contact, roleId, "SYSTEM");
        }
        public async Task UpdateUserAsync(int id, string name, string contact, int roleId)
        {
            if (id <= 0)
                throw new Exception("Id inválido");

            await _repository.UpdateUserAsync(id, name, contact, roleId, "SYSTEM");
        }
        public async Task AssignAreaAsync(int userId, int areaId)
        {
            if (userId <= 0 || areaId <= 0)
                throw new Exception("Datos inválidos");

            await _repository.AssignAreaAsync(userId, areaId, "SYSTEM");
        }
        public async Task<List<UserGridDto>> GetLastUsersAsync()
        {
            return await _repository.GetLastUsersAsync();
        }
        public async Task DeleteUserAsync(int userId)
        {
            if (userId <= 0)
                throw new Exception("Usuario inválido");

            await _repository.DeleteUserAsync(userId, "SYSTEM");
        }
    }
}
