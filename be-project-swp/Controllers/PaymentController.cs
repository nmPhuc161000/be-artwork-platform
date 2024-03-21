/*using be_project_swp.Core.Dtos.PayPal;
using be_project_swp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;


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
        public async Task<IActionResult> CreatePaymentUrl(PaymentInformationModel model)
        {
            var url = await _payPalService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }

        [HttpPost]
        [Route("callback-payment")]
        public IActionResult PaymentCallback()
        {
            var response = _payPalService.PaymentExecute(Request.Query);

            return Json(response);
        }
    }
}
*/