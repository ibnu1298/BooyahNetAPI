using BooyahNetAPI.Authentication;
using BooyahNetAPI.Dtos;
using BooyahNetAPI.Dtos.User;
using BooyahNetAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BooyahNetAPI.DAL
{
    public interface IAdmin
    {
        Task CreateRole(CreateRoleDTO model);
        Task Delete(string id);
        Task<IEnumerable<CustomRole>> GetRole();
        Task<IEnumerable<User>> GetUserRole();
        Task<CustomRole> Update(CustomRole obj);
        Task<CustomRole> GetByName(string name);
        Task<User> GetByNameUser(string name);
        Task AddUserToRole(UserRoleDTO obj);
    }
    public class AdminDAL : IAdmin
    {
        private readonly DataContext _context;
        private readonly RoleManager<CustomRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public AdminDAL(UserManager<User> userManager, RoleManager<CustomRole> roleManager, DataContext context)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task AddUserToRole(UserRoleDTO obj)
        {
            var user = await _userManager.FindByNameAsync(obj.Username);
            try
            {
                await _userManager.AddToRoleAsync(user, obj.Name);
            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal Menambahkan Role Ke User");
            }
        }
        public async Task RemoveRoleFromUser(UserRoleDTO obj)
        {
            var user = await _userManager.FindByNameAsync(obj.Username);
            try
            {
                await _userManager.RemoveFromRoleAsync(user, obj.Name);
            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal Menambahkan Role Ke User");
            }
        }

        public async Task CreateRole(CreateRoleDTO model)
        {

            CustomRole myRole = new CustomRole
            {
                Name = model.Name.ToLower()
            };
            var result = await _roleManager.CreateAsync(myRole);
            if (!result.Succeeded)
            {
                throw new Exception($"Role {model.Name} gagal ditambahkan");
            }
        }

        public async Task Delete(string id)
        {
            try
            {
                var delete = await _roleManager.Roles.FirstOrDefaultAsync(s => s.Id == id);
                if (delete == null) throw new Exception($"Data Student dengan Id {id} tidak ditemukan");
                _roleManager.DeleteAsync(delete);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<CustomRole> GetByName(string name)
        {
            var result = await _roleManager.FindByNameAsync(name);
            if (result == null)
                throw new Exception($"Role dengan Nama {name} tidak ditemukan");
            return result;
        }
        public async Task<User> GetByNameUser(string email)
        {

            var result = await _userManager.Users.Include(u => u.UserRoles).ThenInclude(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
            if (result == null)
                throw new Exception($"User dengan Nama {email} tidak ditemukan");
            return result;
        }

        public async Task<IEnumerable<CustomRole>> GetRole()
        {
            var results = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
            return results;
        }

        public async Task<IEnumerable<User>> GetUserRole()
        {
            var users = await _context.Users.Include(u => u.UserRoles).ThenInclude(r => r.Role).ToListAsync();
            return users;
        }

        public async Task<CustomRole> Update(CustomRole obj)
        {
            try
            {
                var update = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == obj.Id);
                if (update == null) throw new($"Data Tidak dengan Id {obj.Id} Tidak ditemukan");
                update.Name = obj.Name;
                await _roleManager.UpdateAsync(update);
                return obj;

            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
