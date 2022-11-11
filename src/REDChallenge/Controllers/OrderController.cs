using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REDChallenge.Application.Models;
using REDChallenge.Application.ServiceInterface;
using REDChallenge.Domain.ErrorObjects;

namespace REDChallenge.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        #region GET Methods
        [HttpGet("{orderId:Guid}")]
        [ProducesErrorResponseType(typeof(StandardErrorMessage))]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<OrderModel>> Get(Guid orderId)
        {
            return Ok(await _orderService.GetOrderById(orderId));
        }

        [HttpGet("search")]
        [ProducesErrorResponseType(typeof(StandardErrorMessage))]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<OrderModel>> Search([FromQuery]int? orderTypeId, [FromQuery]string? customer, [FromQuery]string? createdBy)
        {
            
            return Ok(await _orderService.SearchOrders(new SearchOrderModel
            {
                CreatedBy = createdBy ?? string.Empty,
                CustomerName = customer ?? string.Empty,
                OrderType = orderTypeId
            }));
        }
        #endregion

        #region POST Methods
        [HttpPost]
        [ProducesErrorResponseType(typeof(StandardErrorMessage))]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        public async Task<ActionResult<OrderModel>> CreateOrder([FromServices]IValidator<CreateOrderModel> validator, [FromBody]CreateOrderModel createOrder)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            await validator.ValidateAndThrowAsync(createOrder);
            return Ok(await _orderService.CreateOrder(createOrder, new Guid(userId)));
        }
        #endregion

        #region PUT Methods
        [HttpPut]
        [ApiConventionMethod(typeof(DefaultApiConventions),
             nameof(DefaultApiConventions.Put))]
        public async Task<ActionResult<OrderModel>> UpdateOrder(UpdateOrderModel createOrder)
        {
            return Ok(await _orderService.UpdateOrder(createOrder));
        }
        #endregion

        #region DELETE Methods
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Delete))]
        public async Task<ActionResult> DeleteOrder(Guid orderId)
        {
            await _orderService.DeleteOrder(orderId);
            return NoContent();
        }
        #endregion
    }
}
