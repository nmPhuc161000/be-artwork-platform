using be_artwork_sharing_platform.Core.Dtos.RequestOrder;
using be_artwork_sharing_platform.Core.Entities;
using be_project_swp.Core.Dtos.RequestOrder;
using be_project_swp.Core.Dtos.Response;

namespace be_artwork_sharing_platform.Core.Interfaces
{
    public interface IRequestOrderService
    {
        Task<RequestOrderDto> GetRequestById(long id);
        Task<GeneralServiceResponseDto> SendRequesrOrder(SendRequest sendRequest, string userId_Sender, string nickName_Sender, string nickName_Receivier);
        IEnumerable<RequestOrderDto> GetMineRequestByNickName(string nickName);
        Task<RequestOrderDto> GetMineRequestById(long id, string userId);
        IEnumerable<ReceiveRequestDto> GetMineOrderByNickName(string nickName);
        Task<GeneralServiceResponseDto> AcceptRquest(long id, UpdateRequest updateRequest, string nickName_Receivier, string userId);
        Task<GeneralServiceResponseDto> CancelRequestByReceivier(long id, CancelRequest cancelRequest, string nickName_Receivier, string userId);
        Task UpdateStatusRequest(long id, string user_Id, UpdateStatusRequest updateStatusRequest);
        int DeleteRequestBySender(long id, string user_Name);
        StatusRequest GetStatusRequestByUserNameRequest(long id, string userNames);
        Task<bool> GetActiveRequestByUserNameRequest(long id, string userNames);
    }
}
