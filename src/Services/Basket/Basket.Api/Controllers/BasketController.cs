using AutoMapper;
using Basket.Api.Entities;
using Basket.Api.GrpcServices;
using Basket.Api.Repository;
using ErrorBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Basket.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly RebateGrpcService _rebateGrpcService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        public BasketController(IBasketRepository basketRepository, RebateGrpcService rebateGrpcService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            try
            {
                _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
                _rebateGrpcService = rebateGrpcService ?? throw new ArgumentNullException(nameof(rebateGrpcService));
                _mapper = mapper ?? throw new ArgumentNullException();
                _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException();
            }
            catch (Exception e)
            {
                Console.WriteLine("Rebate: {0}", _rebateGrpcService);
                Console.WriteLine("Exception: {0}", e.Message);
            }

        }
        // GET: api/<BasketController>
        [HttpGet("{Name:length(24)}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ShoppingCart>> GetBasketByName(string Name)
        {
            var cart = await _basketRepository.GetBasket(Name);
            if (cart != null)
            {
                return Ok(cart);
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateCart([FromBody] ShoppingCart cart)
        {
            //TODO: Communicate with Discount.GRPS
            foreach (var item in cart.Items)
            {
                var rebate = await _rebateGrpcService.GetRebate(item.ProductName);
                if (rebate != null)
                {
                    item.Price -= rebate.Amount;

                }
            }
            // and Calculate latest prices of product into shopping cart
            // consume Discount Grpc
            return Ok(await _basketRepository.UpdateBasket(cart));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _basketRepository.DeleteBasket(userName);
            return Ok();
        }
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            //get existing basket with the TotalPrice

            var basket = await _basketRepository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }
            //create basketCheckoutEvent - set the TotalPrice on the basketCheckout eventMessage
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket?.TotalPrice;

            //send checkout event to rabbitmq
            await _publishEndpoint.Publish(eventMessage);

            //remove the basket
            await _basketRepository.DeleteBasket(basketCheckout.UserName);
            return Accepted();
        }
    }
}
