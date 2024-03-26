using be_project_swp.Core.Dtos.PayPal;
using be_project_swp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PayPal.Api;
using System.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


namespace be_project_swp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPayPalService _payPalService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PaymentController(IPayPalService payPalService, HttpClient httpClient, IConfiguration configuration)
        {
            _payPalService = payPalService;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        /*       [HttpPost]
               [Route("create-payment")]
               [Authorize]
               public async Task<IActionResult> CreateOrder(decimal amount)
               {
                   var response = await _payPalService.CreateOrder(amount);

                   if (response != null && response.IsSuccessStatusCode)
                   {
                       var jsonResponse = await response.Content.ReadAsStringAsync();
                       var responseObject = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);
                       if (responseObject.TryGetValue("links", out var links))
                       {
                           if (links is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
                           {
                               return Ok(new { Links = links });
                           }
                           else
                           {
                               var orderId = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonResponse)["id"];
                               return Ok(new { OrderId = orderId });
                           }
                       }
                       else
                       {
                           var orderId = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonResponse)["id"];
                           return Ok(new { OrderId = orderId });
                       }
                   }
                   return BadRequest();
               }

               [HttpPost]
               [Route("capture-payment")]
               [Authorize]
               public async Task<IActionResult> CapturePayment(string orderId)
               {
                   var result = await _payPalService.CapturePayment(orderId);
                   return StatusCode(result.StatusCode, result.Message);
               }*/

        /*        [HttpPost]
                [Route("create-payment")]
                [Authorize]
                public async Task<IActionResult> CreatePayment(decimal amount)
                {
                    try
                    {
                        var orderResponse = await _payPalService.CreateOrder(amount);
                        return Ok(orderResponse);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new { Message = ex.Message });
                    }
                }

                [HttpPost]
                [Route("capture-payment")]
                [Authorize]
                public async Task<IActionResult> CapturePayment(string orderId)
                {
                    try
                    {
                        var request = new HttpRequestMessage(HttpMethod.Post, $"https://api.sandbox.paypal.com/v2/checkout/orders/{orderId}/capture");
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _payPalService.GetAccessToken());
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var response = await _httpClient.SendAsync(request);

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonResponse = await response.Content.ReadAsStringAsync();
                            return Ok(jsonResponse);
                        }
                        else
                        {
                            return BadRequest(new { Message = "Failed to capture payment." });
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new { Message = ex.Message });
                    }
                }*/

        [HttpPost]
        [Route("create-payment")]
        [Authorize]
        public async Task<IActionResult> CreatePayment(decimal amount)
        {
            try
            {
                var orderResponse = await _payPalService.CreateOrder(amount);
                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("capture-payment")]
        [Authorize]
        public async Task<IActionResult> CapturePayment(string orderId)
        {
            try
            {
                var response = await SendCaptureRequest(orderId);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return Ok(jsonResponse);
                }
                else
                {
                    return BadRequest(new { Message = "Failed to capture payment." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        private async Task<HttpResponseMessage> SendCaptureRequest(string orderId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://api.sandbox.paypal.com/v2/checkout/orders/{orderId}/capture");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _payPalService.GetAccessToken());
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return await _httpClient.SendAsync(request);
        }
    }
}
