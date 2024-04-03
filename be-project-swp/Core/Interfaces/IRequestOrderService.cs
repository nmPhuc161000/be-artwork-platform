using be_artwork_sharing_platform.Core.Dtos.RequestOrder;
using be_artwork_sharing_platform.Core.Entities;
using be_project_swp.Core.Dtos.RequestOrder;

namespace be_artwork_sharing_platform.Core.Interfaces
{
    public interface IRequestOrderService
    {
        Task<RequestOrderDto> GetRequestById(long id);
        Task SendRequesrOrder(SendRequest sendRequest, string userId_Sender, string nickName_Sender, string nickName_Receivier);
        IEnumerable<RequestOrderDto> GetMineRequestByNickName(string nickName);
        IEnumerable<ReceiveRequestDto> GetMineOrderByNickName(string nickName);
        Task UpdateRquest(long id, UpdateRequest updateRequest, string user_Id);
        Task CancelRequestByReceivier(long id, CancelRequest cancelRequest, string user_Id);
        Task UpdateStatusRequest(long id, string user_Id, UpdateStatusRequest updateStatusRequest);
        int DeleteRequestBySender(long id, string user_Name);
        StatusRequest GetStatusRequestByUserNameRequest(long id, string userNames);
        Task<bool> GetActiveRequestByUserNameRequest(long id, string userNames);
    }
}
