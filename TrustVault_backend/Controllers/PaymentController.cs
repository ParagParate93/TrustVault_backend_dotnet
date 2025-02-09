using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TrustVault_backend.Services.Implementation;

namespace TrustVault_backend.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentController(IConfiguration configuration)
        {
            _paymentService = new PaymentService(configuration);
        }
        [EnableCors("AllowFrontend")]
        [HttpPost("createOrder")]
        public IActionResult CreateOrder([FromBody] PaymentRequest request)
        {
            var orderId = _paymentService.CreateOrder(request.Amount);
            return Ok(new { orderId });
        }
    }

    public class PaymentRequest
    {
        public int Amount { get; set; }
    }
}
