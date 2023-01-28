using Basket.API.Entities;
using Basket.API.GrpcService;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : Controller
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGrpService _discoutGrpcService;

        public BasketController(IBasketRepository repository, DiscountGrpService discoutGrpcService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _discoutGrpcService = discoutGrpcService ?? throw new ArgumentNullException(nameof(discoutGrpcService));
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasketAsync(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            // TODO: Communicate with Discount.Grpc and Calculate latest prices of product into shopping cart
            // consume Discount Grpc

            foreach (var item in basket.Items) 
            {
                var coupon = await _discoutGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            return Ok(await _repository.UpdateBasketAsync(basket));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasketAsync(userName);
            return Ok();
        }
    }
}
