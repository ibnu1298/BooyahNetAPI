using BooyahNetAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooyahNetAPI.Data.DAL
{
    public interface IPayment : ICrud<Payment>
    { 
    }
    public class PaymentDAL : IPayment
    {
        private readonly DataContext _context;
        public PaymentDAL(DataContext context)
        {
            _context = context;
        }
        public async Task<Payment> Insert(Payment obj)
        {
            try
            {
                _context.Payments.Add(obj);
                await _context.SaveChangesAsync();
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
        public async Task<Payment> Update(Payment obj)
        {
            try
            {
                var update = await _context.Payments.FirstOrDefaultAsync(s => s.Id == obj.Id);
                if (update == null) throw new Exception($"Data dengan ID = {obj.Id} Tidak ditemukan");
                update.PricePayment = obj.PricePayment;
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
                var delete = await _context.Payments.FirstOrDefaultAsync(s => s.Id == id);
                if (delete == null) throw new Exception($"Data Payment dengan Id {id} tidak ditemukan");
                _context.Payments.Remove(delete);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
        public async Task<IEnumerable<Payment>> GetAll()
        {
            var results = await _context.Payments.OrderBy(c => c.Id).ToListAsync();
            return results;
        }
        public async Task<Payment> GetById(int id)
        {
            var result = await _context.Payments.FirstOrDefaultAsync(c => c.Id == id);
            if (result == null) throw new Exception($"Tidak ada data dengan Id = {id}");
            return result;
        }
        public async Task<IEnumerable<Payment>> GetByName(string name)
        {
            var results = await _context.Payments.Where(c => c.User.FirstName.Contains(name) || c.User.LastName.Contains(name)).OrderBy(c => c.User.FirstName).ToListAsync();
            return results;
        }
    }
}
