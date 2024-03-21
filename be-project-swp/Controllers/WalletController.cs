using be_artwork_sharing_platform.Core.DbContext;
using be_artwork_sharing_platform.Core.Interfaces;
using be_project_swp.Core.Dtos.Wallet;
using be_project_swp.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace be_project_swp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;

        public WalletController(ApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateWallet()
        {
            try
            {
                // Check if the user already has a wallet
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                var existingWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.User_Id == userId);
                if (existingWallet != null)
                {
                    return Conflict("Wallet already exists for this user");
                }

                // Create a new wallet for the user
                var newWallet = new Wallet
                {
                    User_Id = userId,
                    Balance = 0,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                _context.Wallets.Add(newWallet);
                await _context.SaveChangesAsync();

                return Ok("Wallet created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(double amount)
        {
            // Retrieve user's wallet
            string userName = HttpContext.User.Identity.Name;
            string userId = await _authService.GetCurrentUserId(userName);
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.User_Id == userId);
            if (wallet == null)
            {
                return NotFound("Wallet not found");
            }

            var paymentResult = await ProcessPayPalDeposit(amount);

            if (paymentResult.Success)
            {
                wallet.Balance += amount;
                await _context.SaveChangesAsync();

                return Ok("Deposit successful");
            }
            else
            {
                return BadRequest(paymentResult.ErrorMessage);
            }
        }

        private async Task<PaymentResult> ProcessPayPalDeposit(double amount)
        {
            try
            {
                var paypalApiUrl = "https://api.sandbox.paypal.com"; 
                var requestBody = new
                {
                    amount = new
                    {
                        value = amount,
                        currency_code = "USD"
                    }
                };
                var jsonRequest = JsonConvert.SerializeObject(requestBody);
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(paypalApiUrl);

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YOUR_PAYPAL_ACCESS_TOKEN");

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await httpClient.PostAsync("/v2/payments", new StringContent(jsonRequest, Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        return new PaymentResult { Success = true };
                    }
                    else
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        return new PaymentResult { Success = false, ErrorMessage = errorResponse };
                    }
                }
            }
            catch (Exception ex)
            {
                return new PaymentResult { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
