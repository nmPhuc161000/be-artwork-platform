using be_artwork_sharing_platform.Core.Dtos.General;
using be_project_swp.Core.Dtos.PayPal;
using System.Threading.Tasks;

namespace be_project_swp.Core.Interfaces
{
    public interface IPayPalService
    {
        Task<OrderResponse> CreateOrder(decimal amount);
/*        Task<GeneralServiceResponseDto> CapturePayment(string orderId);*/
        Task<string> GetAccessToken();
    }
}
