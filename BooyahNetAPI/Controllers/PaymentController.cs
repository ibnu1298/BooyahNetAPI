using AutoMapper;
using BooyahNetAPI.Data.DAL;
using BooyahNetAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BooyahNetAPI.Dtos.Customer;
using BooyahNetAPI.Models;
using BooyahNetAPI.Dtos.Payment;

namespace BooyahNetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPayment _paymentDAL;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public PaymentController(IPayment paymentDAL, IMapper mapper, DataContext context)
        {
            _paymentDAL = paymentDAL;
            _mapper = mapper;
            _context = context;
        }
        [HttpGet]
        public async Task<IEnumerable<ReadPaymentDTO>> Get()
        {
            var results = await _paymentDAL.GetAll();
            var customer = _mapper.Map<IEnumerable<ReadPaymentDTO>>(results);
            return customer;
        }
        [HttpPost]
        public async Task<ActionResult> Post(CreatePaymentDTO obj)
        {
            try
            {
                var newCust = _mapper.Map<Payment>(obj);
                var result = await _paymentDAL.Insert(newCust);
                var readDTO = _mapper.Map<ReadPaymentDTO>(result);

                return CreatedAtAction("Get", new { id = result.Id }, readDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
