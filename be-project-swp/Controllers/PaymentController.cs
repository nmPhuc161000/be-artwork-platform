using be_project_swp.Core.Dtos.PayPal;
using be_project_swp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace be_project_swp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPayPalService _payPalService;

        public PaymentController(IPayPalService payPalService)
        {
            _payPalService = payPalService;
        }

        [HttpPost]
        [Route("create-payment")]
        public async Task<IActionResult> CreateOrder(string currency, decimal amount)
        {
            var response = await _payPalService.CreateOrder(currency, amount);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var orderId = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonResponse)["id"];
                return Ok(new { OrderId = orderId });
            }

            return BadRequest();
        }
    }
}
