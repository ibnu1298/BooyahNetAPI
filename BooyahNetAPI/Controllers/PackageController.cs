using AutoMapper;
using BooyahNetAPI.Data.DAL;
using BooyahNetAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BooyahNetAPI.Dtos.Customer;
using BooyahNetAPI.Models;
using Microsoft.EntityFrameworkCore;
using BooyahNetAPI.Dtos.Package;

namespace BooyahNetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackage _packageDAL;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public PackageController(IPackage packageDAL, IMapper mapper, DataContext context)
        {
            _packageDAL = packageDAL;
            _mapper = mapper;
            _context = context;
        }
        [HttpGet]
        public async Task<IEnumerable<ReadPackageDTO>> Get()
        {
            var results = await _packageDAL.GetAll();
            var data = _mapper.Map<IEnumerable<ReadPackageDTO>>(results);
            return data;
        }
        [HttpPost]
        public async Task<ActionResult> Post(CreatePackageDTO obj)
        {
            try
            {
                var newData = _mapper.Map<Package>(obj);
                var result = await _packageDAL.Insert(newData);
                var customerDTO = _mapper.Map<ReadPackageDTO>(result);

                return CreatedAtAction("Get", new { id = result.Id }, customerDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult<List<ReadPackageDTO>>> Edit(ReadPackageDTO obj)
        {
            try
            {
                var edit = await _context.Packages.FirstOrDefaultAsync(c => c.Id == obj.Id);
                if (edit == null)
                    return BadRequest($"Package dengan Id = {obj.Id} Tidak ditemukan");
                edit.PackageName = obj.PackageName;
                edit.PricePackage = obj.PricePackage;
                edit.MaxBandwidth = obj.MaxBandwidth;
                edit.MaxUser = obj.MaxUser;
                await _context.SaveChangesAsync();
                return Ok(obj);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
