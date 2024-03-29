using be_artwork_sharing_platform.Core.Dtos.General;
using be_project_swp.Core.Dtos.PayPal;
using System.Threading.Tasks;

namespace be_project_swp.Core.Interfaces
{
    public interface IPayPalService
    {
        Task<OrderAndTokenResponse> CreateOrder(string user_Id, long artwork_Id, string nickName);
/*        Task<GeneralServiceResponseDto> CapturePayment(string orderId);*/
        Task<string> GetAccessToken();
        Task<bool> IsPaymentCaptured(string orderId, string user_Id, long artwork_Id, string nickName);
        Task<bool> IsOrderCreated(string orderId);
    }
}
