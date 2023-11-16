using BooyahNetAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooyahNetAPI.Data.DAL
{
    public interface IPackage : ICrud<Package>
    {
    }
    public class PackageDAL : IPackage
    {
        private readonly DataContext _context;
        public PackageDAL(DataContext context)
        {
            _context = context;
        }
        public async Task<Package> Insert(Package obj)
        {
            try
            {
                _context.Packages.Add(obj);
                await _context.SaveChangesAsync();
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
        public async Task<Package> Update(Package obj)
        {
            try
            {
                var update = await _context.Packages.FirstOrDefaultAsync(s => s.Id == obj.Id);
                if (update == null) throw new Exception($"Data dengan ID = {obj.Id} Tidak ditemukan");
                update.Payments = obj.Payments;
                await _context.SaveChangesAsync();
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
        public async Task Delete(int id)
        {
            try
            {
                var delete = await _context.Packages.FirstOrDefaultAsync(s => s.Id == id);
                if (delete == null) throw new Exception($"Data Package dengan Id {id} tidak ditemukan");
                _context.Packages.Remove(delete);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
        public async Task<IEnumerable<Package>> GetAll()
        {
            var results = await _context.Packages.OrderBy(c => c.PackageName).ToListAsync();
            return results;
        }
        public async Task<Package> GetById(int id)
        {
            var result = await _context.Packages.FirstOrDefaultAsync(c => c.Id == id);
            if (result == null) throw new Exception($"Tidak ada data dengan Id = {id}");
            return result;
        }
        public async Task<IEnumerable<Package>> GetByName(string name)
        {
            var results = await _context.Packages.Where(c => c.PackageName.Contains(name)).OrderBy(c => c.PackageName).ToListAsync();
            return results;
        }
    }
}
