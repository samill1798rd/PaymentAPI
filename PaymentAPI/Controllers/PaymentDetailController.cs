using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentDetailController : ControllerBase
    {
        private readonly PaymentDetailContext _UnitOfWork;
        public PaymentDetailController(PaymentDetailContext UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDetail>>> GetPaymentDetails()
        {
            return await _UnitOfWork.PaymentDetails.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDetail>> GetPaymentDetail(int id)
        {
            var paymentDetail = await _UnitOfWork.PaymentDetails.FindAsync(id);

            if (paymentDetail == null)
            {
                return NotFound();
            }
            else
            {
                return paymentDetail;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentDetail(int id, PaymentDetail paymentDetail)
        {

            if (id != paymentDetail.PaymentDetailId)
            {
                return BadRequest();
            }

            _UnitOfWork.Entry(paymentDetail).State = EntityState.Modified;

            try
            {
                await _UnitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentDetailExits(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }

            return NoContent();

        }

        [HttpPost]
        public async Task<ActionResult<PaymentDetail>> PostPaymentDetail(PaymentDetail paymentDetail)
        {
            _UnitOfWork.PaymentDetails.Add(paymentDetail);

            await _UnitOfWork.SaveChangesAsync();

            return CreatedAtAction("GetPaymentDetail", new { id = paymentDetail.PaymentDetailId }, paymentDetail);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentDetail(int id)
        {
            var paymentDetail = await _UnitOfWork.PaymentDetails.FindAsync(id);

            if (paymentDetail == null)
            {
                return NotFound();
            }
            _UnitOfWork.PaymentDetails.Remove(paymentDetail);

            await _UnitOfWork.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentDetailExits(int id)
        {
            return _UnitOfWork.PaymentDetails.Any(x => x.PaymentDetailId == id);
        }


    }
}
