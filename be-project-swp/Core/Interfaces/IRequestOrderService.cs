using be_artwork_sharing_platform.Core.Dtos.Artwork;
using be_artwork_sharing_platform.Core.Dtos.RequestOrder;
using be_artwork_sharing_platform.Core.Entities;
using be_project_swp.Core.Dtos.RequestOrder;

namespace be_artwork_sharing_platform.Core.Interfaces
{
    public interface IRequestOrderService
    {
        Task SendRequesrOrder(SendRequest sendRequest, string userName_Request, string userId_Receivier, string fullName_Sender, string fullName_Receivier);
        IEnumerable<ReceiveRequestDto> GetMineOrderByUserId(string user_Name);
        IEnumerable<RequestOrderDto> GetMineRequestByUserName(string user_Id);
        Task UpdateRquest(long id, UpdateRequest updateRequest);
        Task CancelRequest (long id);
        Task<bool> GetStatusRequestByUserNameRequest(long id);
    }
}
