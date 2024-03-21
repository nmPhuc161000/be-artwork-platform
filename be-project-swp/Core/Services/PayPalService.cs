/*using System.Net;
using PayPalCheckoutSdk.Orders;
using be_project_swp.Core.Interfaces;
using be_artwork_sharing_platform.Core.DbContext;
using be_project_swp.Core.Dtos.PayPal;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Payments;
using PayPal.Api;
using Payer = PayPalCheckoutSdk.Orders.Payer;
using Item = PayPal.Api.Item;

namespace be_project_swp.Core.Services
{
    public class PayPalService : IPayPalService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public PayPalService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private const double ExchangeRate = 22_863.0;
        public static double ConvertVndToDollar(double vnd)
        {
            var total = Math.Round(vnd / ExchangeRate, 2);
            return total;
        }

        public async Task<string> CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            var envSandbox = new SandboxEnvironment(_configuration["Paypal:ClientId"], _configuration["Paypal:SecretKey"]);
            var client = new PayPalHttpClient(envSandbox);
            var paypalOrderId = DateTime.Now.Ticks;
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];
            var payment = new Payment()
            {
                intent = "sale",
                transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        amount = new Amount()
                        {
                            total = ConvertVndToDollar(model.Amount).ToString(),
                            currency = "USD",
                            details = new AmountDetails
                            {
                                Tax = "0",
                                Shipping = "0",
                                Subtotal = ConvertVndToDollar(model.Amount).ToString(),
                            }
                        },
                        item_list = new ItemList()
                        {
                            items = new List<Item>()
                            {
                                new Item()
                                {
                                    name = " | Order: " + model.OrderDescription,
                                    currency = "USD",
                                    price = ConvertVndToDollar(model.Amount).ToString(),
                                    quantity = 1.ToString(),
                                    sku = "sku",
                                    tax = "0",
                                    url = "https://www.code-mega.com" // Url detail of Item
                                }
                            }
                        },
                        description = $"Invoice #{model.OrderDescription}",
                        invoice_number = paypalOrderId.ToString()
                    }
                },
                redirect_urls = new RedirectUrls()
                {
                    return_url = $"{urlCallBack}?payment_method=PayPal&success=1&order_id={paypalOrderId}",
                    cancel_url = $"{urlCallBack}?payment_method=PayPal&success=0&order_id={paypalOrderId}"
                },
                payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            var request = new PaymentCreatRequest();
            request.RequestBody(payment);

            var paymentUrl = "";
            var response = await client.Execute(request);
            var statusCode = response.StatusCode;

            if (statusCode is not (HttpStatusCode.Accepted or HttpStatusCode.OK or HttpStatusCode.Created))
                return paymentUrl;

            var result = response.Result<Payment>();
            using var links = result.Links.GetEnumerator();

            while (links.MoveNext())
            {
                var lnk = links.Current;
                if (lnk == null) continue;
                if (!lnk.Rel.ToLower().Trim().Equals("approval_url")) continue;
                paymentUrl = lnk.Href;
            }
            return paymentUrl;
        }

        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var response = new PaymentResponseModel();

            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("order_description"))
                {
                    response.OrderDescription = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("transaction_id"))
                {
                    response.TransactionId = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("order_id"))
                {
                    response.OrderId = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("payment_method"))
                {
                    response.PaymentMethod = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("success"))
                {
                    response.Success = Convert.ToInt32(value) > 0;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("paymentid"))
                {
                    response.PaymentId = value;
                }

                if (!string.IsNullOrEmpty(key) && key.ToLower().Equals("payerid"))
                {
                    response.PayerId = value;
                }
            }
            return response;
        }
    }   
}
*/