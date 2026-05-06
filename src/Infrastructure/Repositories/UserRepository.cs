using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateUserAsync(string name, string contact, int roleId, string performedBy)
        {

            var parameters = new[]
            {
                new SqlParameter("@Name", name),
                new SqlParameter("@ContactInfo", contact),
                new SqlParameter("@RoleId", roleId),
                new SqlParameter("@PerformedBy", performedBy)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_CreateUser @Name, @ContactInfo, @RoleId, @PerformedBy",
                parameters);
        }
        public async Task UpdateUserAsync(int userId, string name, string contact, int roleId, string performedBy)
        {
            var parameters = new[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@Name", name),
                new SqlParameter("@ContactInfo", contact),
                new SqlParameter("@RoleId", roleId),
                new SqlParameter("@PerformedBy", performedBy)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_UpdateUser @UserId, @Name, @ContactInfo, @RoleId, @PerformedBy",
                parameters);
        }
        public async Task AssignAreaAsync(int userId, int areaId, string performedBy)
        {
            var parameters = new[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@AreaId", areaId),
                new SqlParameter("@PerformedBy", performedBy)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_AssignArea @UserId, @AreaId, @PerformedBy",
                parameters);
        }
        public async Task<List<UserGridDto>> GetLastUsersAsync()
        {
            return await (
                from u in _context.Users
                where u.Status == true

                join r in _context.Roles on u.RoleId equals r.Id

                join ua in _context.UserAreas on u.Id equals ua.UserId into uaGroup
                from ua in uaGroup.DefaultIfEmpty()

                join a in _context.Areas on ua.AreaId equals a.Id into aGroup
                from a in aGroup.DefaultIfEmpty()

                orderby u.CreatedAt descending

                select new UserGridDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    ContactInfo = u.ContactInfo,
                    RoleId = u.RoleId,              
                    RoleName = r.Name,
                    AreaId = ua != null ? ua.AreaId : null,
                    AreaName = a != null ? a.Name : "Sin asignar",
                    Status = u.Status,
                    CreatedAt = u.CreatedAt
                }
                
            ).Take(10).ToListAsync();
        }
        public async Task DeleteUserAsync(int userId, string performedBy)
        {
            var parameters = new[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@PerformedBy", performedBy)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_DeleteUser @UserId, @PerformedBy",
                parameters);
        }
    }
}
