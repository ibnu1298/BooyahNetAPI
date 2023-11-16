using BooyahNetAPI.Authentication;
using BooyahNetAPI.Dtos.Customer;
using BooyahNetAPI.Dtos.User;
using BooyahNetAPI.Helpers;
using BooyahNetAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BooyahNetAPI.Data.DAL
{ 
    public interface IUser
    {
        Task<User> GetById (string id);
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest obj);
        Task<User> Registration(CreateUserDTO user);
        Task<User> GetByEmailOrUser(string emailOrUsername);
        Task<IEnumerable<User>> GetUsersPayment();
        Task<IEnumerable<User>> GetUsersPaymentPackage(string username);
    }
public class UserDAL : IUser
    {
        private UserManager<User> _userManager;
        private readonly DataContext _context;
        private readonly RoleManager<CustomRole> _roleManager;
        private AppSettings _appSettings;
        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(1),//hanya berlaku 1 Hari
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public UserDAL(DataContext context, IOptions<AppSettings> appSettings, UserManager<User> userManager, RoleManager<CustomRole> roleManager)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<User> GetById(string id)
        {
            var result = await _userManager.Users.Include(u => u.UserRoles).ThenInclude(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
            if (result == null)
                throw new Exception($"User dengan Nama {id} tidak ditemukan");
            return result;
        }
        public async Task<User> Registration(CreateUserDTO user)
        {
            try
            {
                TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                var newUser = new User
                {
                    UserName = user.UserName,
                    Email = user.Email.ToLower(),
                    FirstName = ti.ToTitleCase(user.FirstName.ToLower()),
                    LastName = ti.ToTitleCase(user.LastName.ToLower()),
                    Address = user.Address,
                    Gender = user.Gender,
                    PhoneNumber = user.PhoneNumber,
                };
                #region Validate Username & Email
                var username = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == user.UserName.ToUpper());
                if (username != null) throw new Exception($"Username {user.UserName} sudah digunakan");
                var email = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == user.Email.ToUpper());
                if (email != null) throw new Exception($"Email {user.Email} sudah digunakan");

                #endregion

                var result = await _userManager.CreateAsync(newUser, user.Password);
                if (!result.Succeeded)
                {
                    StringBuilder sb = new StringBuilder();
                    var errors = result.Errors;
                    foreach (var error in errors)
                    {
                        sb.Append($"{error.Code} - {error.Description} \n");
                    }
                    throw new Exception($"Error: {sb.ToString()}");
                }
                //var getRole = await _roleManager.FindByNameAsync("User");
                //await _userManager.AddToRoleAsync(newUser, getRole.Name);
                return newUser;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
        public async Task<User> Update(User obj)
        {
            try
            {
                var update = await _context.Users.FirstOrDefaultAsync(s => s.Id == obj.Id);
                if (update == null) throw new Exception($"Data dengan ID = {obj.Id} Tidak ditemukan");
                update.FirstName = obj.FirstName;
                await _context.SaveChangesAsync();
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
        public async Task<IEnumerable<User>> GetAll()
        {
            var results = await _context.Users.OrderBy(c => c.FirstName).ToListAsync();
            return results;
        }
        public async Task<User> GetByEmailOrUser(string emailOrUsername)
        {
            var result = await _userManager.Users.Include(u => u.UserRoles).ThenInclude(u => u.Role).FirstOrDefaultAsync(u => u.Email == emailOrUsername || u.UserName == emailOrUsername);
            if (result == null)
                throw new Exception($"{emailOrUsername} tidak ditemukan");
            return result;
        }
        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest obj)
        {
            var getUsername = await GetByEmailOrUser(obj.UsernameOrEmail);
            var currUser = await _userManager.FindByNameAsync(getUsername.UserName);
            var userResult = await _userManager.CheckPasswordAsync(currUser, obj.Password);
            if (!userResult)
                throw new Exception("Autentikasi gagal!, Email atau Password Salah");
            var roles = await GetByEmailOrUser(obj.UsernameOrEmail);
            var role = roles.UserRoles.FirstOrDefault();
            JwtSecurityToken generateToken = new JwtSecurityToken(
                claims: new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, currUser.UserName),
                    new Claim(ClaimTypes.Role, $"{role}")},
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(
                    key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret)),
                    algorithm: SecurityAlgorithms.HmacSha256
                )
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(generateToken);
            var user = new AuthenticateResponse(currUser, token);            
            user.Message = "Yey Berhasil Login";
            user.IsSucceeded = true;
           
            return user;

        }
        public async Task<IEnumerable<User>> GetByName(string name)
        {
            var results = await _context.Users.Where(c => c.FirstName.Contains(name) || c.LastName.Contains(name)).OrderBy(c => c.FirstName).ToListAsync();
            return results;
        }
        public async Task<IEnumerable<User>> GetUsersPayment() 
        {
            var results = await _context.Users.Include(c => c.Payments).OrderBy(c => c.FirstName).AsNoTracking().ToListAsync();
            return results;
        }
        public async Task<IEnumerable<User>> GetUsersPaymentPackage(string username) 
        {
            var results = await _context.Users.Include(c => c.Payments).Where(c => c.UserName.Contains(username)).OrderBy(c => c.FirstName).ToListAsync();
            foreach (var payment in results)
            {    
               await _context.Payments.Include(c => c.Package).ToListAsync();                
            }
            return results;
        }



    }
}
