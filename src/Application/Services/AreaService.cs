using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository _repository;
        public AreaService(IAreaRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<Area>> GetAreasAsync()
        {
            return await _repository.GetAreasAsync();
        }
    }
}
