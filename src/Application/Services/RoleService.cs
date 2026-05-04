using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;

        public RoleService(IRoleRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<Role>> GetRolesAsync()
        {
            return await _repository.GetRolesAsync();
        }
    }
}
