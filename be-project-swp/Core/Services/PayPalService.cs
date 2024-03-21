using System.Net;
using PayPalCheckoutSdk.Orders;
using be_project_swp.Core.Interfaces;
using be_artwork_sharing_platform.Core.DbContext;
using be_project_swp.Core.Dtos.PayPal;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Payments;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace be_project_swp.Core.Services
{
    public class PayPalService : IPayPalService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<PayPalSettings> _paypalSettings;
        private readonly IConfiguration _configuration;


        public PayPalService(HttpClient httpClient, IOptions<PayPalSettings> paypalSettings, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _paypalSettings = paypalSettings;
            _configuration = configuration;
        }

        public async Task<HttpResponseMessage> CreateOrder(string currency, decimal amount)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.sandbox.paypal.com/v2/checkout/orders");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var orderRequest = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
            {
                new
                {
                    amount = new
                    {
                        currency_code = currency,
                        value = amount.ToString("0.00")
                    }
                }
            }
            };

            request.Content = new StringContent(JsonSerializer.Serialize(orderRequest), Encoding.UTF8, "application/json");

            return await _httpClient.SendAsync(request);
        }

        private async Task<string> GetAccessToken()
        {
            var client = new HttpClient();
            var byteArray = Encoding.UTF8.GetBytes($"{_configuration["Paypal:ClientId"]}:{_configuration["Paypal:ClientSecret"]}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials"
            };

            var response = await client.PostAsync("https://api.sandbox.paypal.com/v1/oauth2/token", new FormUrlEncodedContent(form));
            var token = JsonSerializer.Deserialize<Dictionary<string, string>>(await response.Content.ReadAsStringAsync())["access_token"];

            return token;
        }
    }
}
