using be_artwork_sharing_platform.Core.DbContext;
using be_artwork_sharing_platform.Core.Dtos.RequestOrder;
using be_artwork_sharing_platform.Core.Entities;
using be_artwork_sharing_platform.Core.Interfaces;
using be_project_swp.Core.Dtos.RequestOrder;
using Microsoft.EntityFrameworkCore;

namespace be_artwork_sharing_platform.Core.Services
{
    public class RequestOrderService : IRequestOrderService
    {
        private readonly ApplicationDbContext _context;

        public RequestOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RequestOrderDto> GetRequestById(long id)
        {
            var request = await _context.RequestOrders.FirstOrDefaultAsync(o => o.Id == id);
            if (request == null)
            {
                return null;
            }
            var requestDto = new RequestOrderDto()
            {
                Id = request.Id,
                NickName_Sender = request.NickName_Sender,
                NickName_Receivier = request.NickName_Receivier,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Text = request.Text,
                CreatedAt = request.CreatedAt,
                StatusRequest = request.StatusRequest,
                IsActive = request.IsActive,
                IsDeleted = request.IsDeleted,
            };
            return requestDto;
        }

        public async Task SendRequesrOrder(SendRequest sendRequest, string userId_Sender, string nickName_Sender, string nickName_Receivier)
        {
            var request = new RequestOrder
            {
                NickName_Sender = nickName_Sender,
                NickName_Receivier = nickName_Receivier,
                UserId_Sender = userId_Sender,
                Email = sendRequest.Email,
                PhoneNumber = sendRequest.PhoneNumber,
                Text = sendRequest.Text
            };
            await _context.RequestOrders.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<ReceiveRequestDto> GetMineOrderByNickName(string nickName)
        {
            var receivier = _context.RequestOrders.Where(f => f.NickName_Receivier == nickName)
                .Select(f => new ReceiveRequestDto
                {
                    Id =f.Id,
                    NickName_Sender = f.NickName_Sender,
                    Email_Sender = f.Email,
                    PhoneNo_Sender = f.PhoneNumber,
                    Text = f.Text,
                    CreatedAt = f.CreatedAt,
                    IsActive = f.IsActive,
                    IsDeleted = f.IsDeleted,
                    StatusRequest = f.StatusRequest.ToString(),
                }).ToList();
            return receivier;
        }

        public IEnumerable<RequestOrderDto> GetMineRequestByNickName(string nickName)
        {
            var request = _context.RequestOrders.Where(f => f.NickName_Sender == nickName)
                .Select(f => new RequestOrderDto
                {
                    Id = f.Id,
                    NickName_Sender = f.NickName_Sender,
                    NickName_Receivier = f.NickName_Receivier,
                    Email = f.Email,
                    PhoneNumber = f.PhoneNumber,
                    Text = f.Text,
                    CreatedAt= f.CreatedAt,
                    IsActive = f.IsActive,
                    IsDeleted = f.IsDeleted,
                    StatusRequest = f.StatusRequest,
                }).ToList();
            return request;
        }

        public async Task UpdateRquest(long id, UpdateRequest updateRequest, string user_Id)
        {
            var request = await _context.RequestOrders.FirstOrDefaultAsync(f => f.Id == id && f.UserId_Sender == user_Id);
            if(request is not null)
            {
                request.IsActive = updateRequest.IsActive;
            }
            _context.Update(request);
            _context.SaveChanges();
        }

        public async Task CancelRequestByReceivier(long id, CancelRequest cancelRequest, string user_Id)
        {
            var request = await _context.RequestOrders.FirstOrDefaultAsync(r => r.Id == id && r.UserId_Sender == user_Id);
            if(request is not null)
            {
                request.IsDeleted = cancelRequest.IsDelete;
            }
            _context.Update(request);
            _context.SaveChanges();
        }

        public async Task UpdateStatusRequest(long id, string user_Id, UpdateStatusRequest updateStatusRequest)
        {
            var request = await _context.RequestOrders.FirstOrDefaultAsync(r => r.Id == id && r.UserId_Sender == user_Id);
            if (request is not null)
            {
                request.StatusRequest = updateStatusRequest.StatusRequest;
            }
            _context.Update(request);
            await _context.SaveChangesAsync();
        }

        public StatusRequest GetStatusRequestByUserNameRequest(long id, string userId)
        {
            var checkStatusRequest = _context.RequestOrders.FirstOrDefault(r => r.Id == id && r.UserId_Sender == userId);
            return checkStatusRequest.StatusRequest;
        }

        public async Task<bool> GetActiveRequestByUserNameRequest(long id, string userId)
        {
            var checkStatusRequest = await _context.RequestOrders.FirstOrDefaultAsync(r => r.Id == id && r.UserId_Sender == userId);
            if(checkStatusRequest is not null)
            {
                return checkStatusRequest.IsActive;
            }
            return false;
        }

        public int DeleteRequestBySender(long id, string userId)
        {
            var request = _context.RequestOrders.FirstOrDefault(o => o.Id == id && o.UserId_Sender == userId);
            if (request is not null)
            {
                _context.Remove(request);
                return _context.SaveChanges();
            }
            return 0;
        }
    }
}
