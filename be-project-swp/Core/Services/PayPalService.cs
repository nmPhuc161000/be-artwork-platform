using be_artwork_sharing_platform.Core.Dtos.General;
using be_project_swp.Core.Dtos.PayPal;
using be_project_swp.Core.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
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

        /*        public async Task<OrderResponse> CreateOrder(decimal amount)
                {
                    string currency = "usd";
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
                }*/

        public async Task<OrderAndTokenResponse> CreateOrder(decimal amount)
        {
            string currency = "usd";
            string accessToken = await GetAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.sandbox.paypal.com/v2/checkout/orders");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
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

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var orderResponse = JsonSerializer.Deserialize<OrderResponse>(jsonResponse);

                return new OrderAndTokenResponse
                {
                    Order = orderResponse,
                    AccessToken = accessToken
                };
            }
            else
            {
                throw new Exception("Failed to create order.");
            }
        }

        public async Task<string> GetAccessToken()
        {
            var client = new HttpClient();
            var byteArray = Encoding.UTF8.GetBytes($"{_configuration["Paypal:ClientId"]}:{_configuration["Paypal:ClientSecret"]}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials"
            };

            var response = await client.PostAsync("https://api.sandbox.paypal.com/v1/oauth2/token", new FormUrlEncodedContent(form));
            var responseBody = await response.Content.ReadAsStringAsync();
            var tokenDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

            if (tokenDictionary.TryGetValue("access_token", out object accessToken))
            {
                return accessToken.ToString();
            }
            else
            {
                throw new Exception("Access token not found in PayPal response.");
            }
        }

        public async Task<GeneralServiceResponseDto> CapturePayment(string orderId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://api.sandbox.paypal.com/v2/checkout/orders/{orderId}/capture");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);
                if (responseObject.TryGetValue("status", out var status) && status.ToString() == "COMPLETED")
                {
                    return new GeneralServiceResponseDto()
                    {
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Payment successfully captured."
                    };
                }
                else
                {
                    return new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 400,
                        Message = "Unable to capture payment."
                    };
                }
            }
            else
            {
                return new GeneralServiceResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 400,
                    Message = "Failed to capture payment."
                };
            }
        }

        public async Task<bool> IsPaymentCaptured(string orderId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.sandbox.paypal.com/v2/checkout/orders/{orderId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent("", Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);
                if (responseObject.TryGetValue("status", out var status) && status.ToString() == "COMPLETED")
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> IsOrderCreated(string orderId)
        {
            var accessToken = await GetAccessToken();

            var request = new HttpRequestMessage(HttpMethod.Post, $"https://api.sandbox.paypal.com/v2/checkout/orders/{orderId}/capture");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent("", Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            // Kiểm tra xem yêu cầu thành công và đơn hàng tồn tại
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false; // Đơn hàng không tồn tại
            }
            else
            {
                throw new Exception("Failed to check order status.");
            }
        }
    }
}
//https://api.sandbox.paypal.com/v1/oauth2/token
//https://sandbox.paypal.com