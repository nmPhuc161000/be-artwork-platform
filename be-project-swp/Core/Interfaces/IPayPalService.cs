using be_project_swp.Core.Dtos.PayPal;

namespace be_project_swp.Core.Interfaces
{
    public interface IPayPalService
    {
        Task<string> CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
