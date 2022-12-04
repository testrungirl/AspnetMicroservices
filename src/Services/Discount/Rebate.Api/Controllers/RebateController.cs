using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rebate.Api.Models;
using Rebate.Api.Repositories;
using System.Net;

namespace Rebate.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RebateController : ControllerBase
    {
        private IRebateRepository _rebateRepository;
        public RebateController(IRebateRepository rebateRepository)
        {
            _rebateRepository= rebateRepository;
        }

        [HttpGet(Name ="GetCouponByName")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.BadGateway)]
        public async Task<IActionResult> GetCoupon(string ProductName)
        {
            if(ProductName == null)
            {
                return BadRequest();
            }
            var couponData = await _rebateRepository.GetRebate(ProductName);
            if (couponData.Success)
            {
                return Ok(couponData.Data);
            }
            return StatusCode(502);
        }
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.BadGateway)]
        public async Task<IActionResult> CreateCoupon(Coupon coupon)
        {
            if(coupon == null)
            {
                return BadRequest();
            }
            var data = await _rebateRepository.CreateRebate(coupon);
            if (data.Success)
            {
                return Ok(coupon);
            }
            return StatusCode(502);
        }
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.BadGateway)]
        public async Task<IActionResult> EditCoupon(Coupon coupon)
        {
            if(coupon == null)
            {
                return BadRequest();
            }
            var data = await _rebateRepository.UpdateRebate(coupon);
            if (data.Success)
            {
                return NoContent();
            }
            return StatusCode(502);
        }

        [HttpDelete(Name ="DeleteCouponByName")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> RemoveCoupon(string ProductName)
        {
            var deleteStatus = await _rebateRepository.DeleteRebate(ProductName);
            if (deleteStatus.Success)
            {
                return Ok(deleteStatus.Success);
            }
            return StatusCode((int)HttpStatusCode.NotFound);

        }
    }
}
