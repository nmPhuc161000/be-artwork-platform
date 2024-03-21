using be_project_swp.Core.Dtos.PayPal;

namespace be_project_swp.Core.Interfaces
{
    public interface IPayPalService
    {
        Task<HttpResponseMessage> CreateOrder(string currency, decimal amount);
    }
}
