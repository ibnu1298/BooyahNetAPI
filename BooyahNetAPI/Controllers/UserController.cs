using AutoMapper;
using BooyahNetAPI.Data.DAL;
using BooyahNetAPI.Data;
using BooyahNetAPI.Dtos.Customer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooyahNetAPI.Authentication;
using BooyahNetAPI.Helpers;
using BooyahNetAPI.Models;
using BooyahNetAPI.Dtos.User;
using Microsoft.AspNetCore.Identity;

namespace BooyahNetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly RoleManager<CustomRole> _roleManager;
        public UserController(IUser user, IMapper mapper, DataContext context, RoleManager<CustomRole> roleManager)
        {
            _mapper = mapper;
            _context = context;
            _user = user;
            _roleManager = roleManager;
        }
        UserDTO userDTO = new UserDTO();

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetById(string id)
        {
           
            try
            {
                var result = await _user.GetById(id);
                userDTO = _mapper.Map<UserDTO>(result);
                if (result != null) {
                    userDTO.IsSucceeded = true;
                    userDTO.Message = $"Pengambilan data User {userDTO.UserName} Berhasil";
                }
                return userDTO;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("EditUserName")]
        public async Task<ActionResult<List<UserUpdateUsernameDTO>>> EditUsername(UserUpdateUsernameDTO obj)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == obj.Id);
                if (user == null)
                    return BadRequest($"ID {obj.Id} Tidak ditemukan");
                user.UserName = obj.Username;
                user.PasswordHash = obj.Password;
                var username = await _context.Users.FirstOrDefaultAsync(c => (c.UserName == obj.Username || c.Email == obj.Username) && c.Id != obj.Id);
                if (username != null)
                    return BadRequest($"{obj.Username} Sudah digunakan");
                var password = await _context.Users.FirstOrDefaultAsync(c => c.PasswordHash == obj.Password);
                if (password == null)
                    return BadRequest($"Password Salah");
                await _context.SaveChangesAsync();
                return Ok(obj);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("EditEmail")]
        public async Task<ActionResult<List<UserUpdateEmailDTO>>> EditEmail(UserUpdateEmailDTO obj)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == obj.Id);
                if (user == null)
                    return BadRequest($"ID {obj.Id} Tidak ditemukan");
                user.Email = obj.Email;
                user.PasswordHash = obj.Password;
                var email = await _context.Users.FirstOrDefaultAsync(c => (c.Email == obj.Email || c.Email == obj.Email) && c.Id != obj.Id);
                if (email != null)
                    return BadRequest($"{obj.Email} Sudah Terdafter");
                var password = await _context.Users.FirstOrDefaultAsync(c => c.PasswordHash == obj.Password);
                if (password == null)
                    return BadRequest($"Password Salah");
                await _context.SaveChangesAsync();
                return Ok(obj);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Login")]
        public async Task<ActionResult> LoginUsernameOrEmail(AuthenticateRequest obj)
        {
            try
            {
                var response = await _user.Authenticate(obj);
                if (response == null)
                    return BadRequest(new { message = "UserName Or Password Is Incorrect" });
                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
          
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(CreateUserDTO obj)
        {
            try
            {
                var result = await _user.Registration(obj);
                userDTO = _mapper.Map<UserDTO>(result);
                if (result == null)
                    userDTO.IsSucceeded = false;
                userDTO.Message = $"Registration Success, Username = {userDTO.UserName}";
                userDTO.IsSucceeded = true;
                return CreatedAtAction("GetById", new { id = result.Id }, userDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("PaymentPackage")]
        public async Task<ActionResult<List<UserPaymentDTO>>> GetUserPayments(string username)
        {
            try
            {
                var obj = await _user.GetByEmailOrUser(username);
                var get = _mapper.Map<User>(obj);
                var result = await _user.GetUsersPaymentPackage(obj.UserName);
                var DTO = _mapper.Map<List<UserPaymentDTO>>(result);

                return DTO;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpPut]
        //public async Task<ActionResult<List<UserDTO>>> Edit(UserDTO obj)
        //{
        //    try
        //    {
        //        var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == obj.Id);
        //        if (user == null)
        //            return BadRequest($"Customer dengan Id = {obj.Id} Tidak ditemukan");
        //        user.FirstName = obj.FirstName;
        //        user.Email = obj.Email;
        //        user.Address = obj.Address;
        //        user.PhoneNumber = obj.PhoneNumber;
        //        await _context.SaveChangesAsync();
        //        return Ok(obj);

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
