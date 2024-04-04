using Amazon.Runtime.Internal.Util;
using be_artwork_sharing_platform.Core.DbContext;
using be_artwork_sharing_platform.Core.Dtos.RequestOrder;
using be_artwork_sharing_platform.Core.Entities;
using be_artwork_sharing_platform.Core.Interfaces;
using be_project_swp.Core.Dtos.Email;
using be_project_swp.Core.Dtos.RequestOrder;
using be_project_swp.Core.Dtos.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace be_artwork_sharing_platform.Core.Services
{
    public class RequestOrderService : IRequestOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogService _logService;

        public RequestOrderService(ApplicationDbContext context, ILogService logService)
        {
            _context = context;
            _logService = logService;
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

        public async Task<GeneralServiceResponseDto> SendRequesrOrder(SendRequest sendRequest, string userId_Sender, string nickName_Sender, string nickName_Receivier, string userName)
        {
            try
            {
                var checkUser = await _context.Users.FirstOrDefaultAsync(u => u.NickName == nickName_Receivier);
                if (checkUser == null)
                {
                    return new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 404,
                        Message = "User not found"
                    };
                }
                else
                {
                    if (nickName_Sender == nickName_Receivier)
                    {
                        return new GeneralServiceResponseDto()
                        {
                            IsSucceed = false,
                            StatusCode = 400,
                            Message = "You can not request you"
                        };
                    }
                    else
                    {
                        var request = new RequestOrder
                        {
                            NickName_Sender = nickName_Sender,
                            NickName_Receivier = nickName_Receivier,
                            UserId_Sender = userId_Sender,
                            Email = sendRequest.Email,
                            PhoneNumber = sendRequest.PhoneNumber,
                            Text = sendRequest.Text,
                        };
                        await _context.RequestOrders.AddAsync(request);
                        await _context.SaveChangesAsync();
                        await _logService.SaveNewLog(userName, "Send Order Request");
                        return new GeneralServiceResponseDto()
                        {
                            IsSucceed = true,
                            StatusCode = 200,
                            Message = "Send Request to Order Artwork Successfully"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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

        public async Task<RequestOrderDto> GetMineRequestById(long id, string userId)
        {
            var request = await _context.RequestOrders.FirstOrDefaultAsync(r => r.Id == id && r.UserId_Sender == userId);
            if(request != null)
            {
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
            return null;
        }

        public async Task<GeneralServiceResponseDto> AcceptRquest(long id, UpdateRequest updateRequest, string nickName_Receivier, string userName)
        {
            try
            {
                var request = await _context.RequestOrders.FirstOrDefaultAsync(r => r.Id == id);
                if (request is null)
                {
                    return new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 404,
                        Message = "Order Request not found"
                    };
                }
                else
                {
                    if (request.NickName_Receivier != nickName_Receivier)
                    {
                        return new GeneralServiceResponseDto()
                        {
                            IsSucceed = false,
                            StatusCode = 400,
                            Message = "You cann't accept this request"
                        };
                    }
                    else
                    {
                        if (request.IsDeleted == true)
                        {
                            return new GeneralServiceResponseDto()
                            {
                                IsSucceed = false,
                                StatusCode = 400,
                                Message = "This request was refuse so you can accept this request"
                            };
                        }
                        else if (request.IsActive == false)
                        {
                            return new GeneralServiceResponseDto()
                            {
                                IsSucceed = false,
                                StatusCode = 400,
                                Message = "This request was accept so you can accept this request again"
                            };
                        }
                        else
                        {
                            request.IsActive = updateRequest.IsActive;
                            _context.Update(request);
                            _context.SaveChanges();
                            await _logService.SaveNewLog(userName, "Accept Request");
                            return new GeneralServiceResponseDto()
                            {
                                IsSucceed = true,
                                StatusCode = 200,
                                Message = "Accept this request successfully"
                            };
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<GeneralServiceResponseDto> CancelRequestByReceivier(long id, CancelRequest cancelRequest, string nickName_Receivier, string userName)
        {
            try
            {
                var request = await _context.RequestOrders.FirstOrDefaultAsync(r => r.Id == id);
                if (request is null)
                {
                    return new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 404,
                        Message = "Order Request not found"
                    };
                }
                else
                {
                    if (request.NickName_Receivier != nickName_Receivier)
                    {
                        return new GeneralServiceResponseDto()
                        {
                            IsSucceed = false,
                            StatusCode = 400,
                            Message = "You cann't refuse this request"
                        };
                    }
                    else
                    {
                        if (request.IsActive == false)
                        {
                            return new GeneralServiceResponseDto()
                            {
                                IsSucceed = false,
                                StatusCode = 400,
                                Message = "This request was accept so you can refuse this request"
                            };
                        }
                        else if (request.IsDeleted == true)
                        {
                            return new GeneralServiceResponseDto()
                            {
                                IsSucceed = false,
                                StatusCode = 400,
                                Message = "This request was refuse so you can refuse this request again"
                            };
                        }
                        else
                        {
                            request.IsDeleted = cancelRequest.IsDelete;
                            _context.Update(request);
                            _context.SaveChanges();
                            await _logService.SaveNewLog(userName, "Cancel Request");
                            return new GeneralServiceResponseDto()
                            {
                                IsSucceed = true,
                                StatusCode = 200,
                                Message = "Refuse this request successfully"
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<GeneralServiceResponseDto> UpdateStatusRequest(long id, string NickName_Receivier, UpdateStatusRequest updateStatusRequest, string userName)
        {
            try
            {
                var request = await _context.RequestOrders.FirstOrDefaultAsync(r => r.Id == id);
                if (request is not null)
                {
                    if (request.NickName_Receivier != NickName_Receivier)
                    {
                        return new GeneralServiceResponseDto()
                        {
                            IsSucceed = false,
                            StatusCode = 400,
                            Message = "You cann't update this status request"
                        };
                    }
                    else
                    {
                        if (request.StatusRequest == updateStatusRequest.StatusRequest)
                        {
                            return new GeneralServiceResponseDto()
                            {
                                IsSucceed = false,
                                StatusCode = 400,
                                Message = "You cann't select same status request"
                            };
                        }
                        else
                        {
                            if (request.StatusRequest == StatusRequest.Waiting)
                            {
                                if (updateStatusRequest.StatusRequest == StatusRequest.Completed)
                                {
                                    return new GeneralServiceResponseDto()
                                    {
                                        IsSucceed = false,
                                        StatusCode = 400,
                                        Message = "You should choose the process in the correct order"
                                    };
                                }
                                else
                                {
                                    await _logService.SaveNewLog(userName, "Update Status Request");
                                    request.StatusRequest = updateStatusRequest.StatusRequest;
                                    _context.RequestOrders.Update(request);
                                    await _context.SaveChangesAsync();
                                    return new GeneralServiceResponseDto()
                                    {
                                        IsSucceed = true,
                                        StatusCode = 200,
                                        Message = "Update Status Successfully"
                                    };
                                }
                            }
                            else if (request.StatusRequest == StatusRequest.Processing)
                            {
                                if (updateStatusRequest.StatusRequest == StatusRequest.Waiting)
                                {
                                    return new GeneralServiceResponseDto()
                                    {
                                        IsSucceed = false,
                                        StatusCode = 400,
                                        Message = "You cannot select the request status in the foreground"
                                    };
                                }
                                else
                                {
                                    await _logService.SaveNewLog(userName, "Update Status Request");
                                    request.StatusRequest = updateStatusRequest.StatusRequest;
                                    _context.Update(request);
                                    await _context.SaveChangesAsync();
                                    return new GeneralServiceResponseDto()
                                    {
                                        IsSucceed = true,
                                        StatusCode = 200,
                                        Message = "Update Status Successfully"
                                    };
                                }
                            }
                            else if (request.StatusRequest == StatusRequest.Completed)
                            {
                                return new GeneralServiceResponseDto()
                                {
                                    IsSucceed = false,
                                    StatusCode = 400,
                                    Message = "Request Order has been completed so you can no longer update your order"
                                };
                            }
                        }
                    }
                }
                return new GeneralServiceResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 404,
                    Message = "Request Order not found"
                };
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
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

        public async Task<int> DeleteRequestBySender(long id, string userId, string userName)
        {
            var request = _context.RequestOrders.FirstOrDefault(o => o.Id == id && o.UserId_Sender == userId);
            if (request is not null)
            {
                await _logService.SaveNewLog(userName, "Delete Request");
                _context.Remove(request);
                return _context.SaveChanges();
            }
            return 0;
        }

        public async Task<GeneralServiceResponseDto> SendResultRequest(SendResultRequest sendResultRequest, long id, string nickName_Receivier, string userName)
        {
            try
            {
                var orderRequest = await _context.RequestOrders.FirstOrDefaultAsync(o => o.Id == id);
                if (orderRequest is null)
                {
                    return new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 404,
                        Message = "Order Request not found"
                    };
                }
                else
                {
                    if (orderRequest.NickName_Receivier != nickName_Receivier)
                    {
                        return new GeneralServiceResponseDto()
                        {
                            IsSucceed = false,
                            StatusCode = 400,
                            Message = "You cann't send result request"
                        };
                    }
                    else
                    {
                        if (orderRequest.StatusRequest != StatusRequest.Completed)
                        {
                            return new GeneralServiceResponseDto()
                            {
                                IsSucceed = false,
                                StatusCode = 400,
                                Message = "The request has not been completed so the results cannot be sent"
                            };
                        }
                        else if(orderRequest.IsPayment == true)
                        {
                            return new GeneralServiceResponseDto()
                            {
                                IsSucceed = false,
                                StatusCode = 400,
                                Message = "The request already sent the result"
                            };
                        }
                        else
                        {
                            orderRequest.Url_Image = sendResultRequest.Url_Image;
                            orderRequest.Price = sendResultRequest.Price;
                            orderRequest.Text_Result = sendResultRequest.Text;
                            orderRequest.IsPayment = true;
                            _context.RequestOrders.Update(orderRequest);
                            await _logService.SaveNewLog(userName, "Send Result Request");
                            await _context.SaveChangesAsync();
                            return new GeneralServiceResponseDto()
                            {
                                IsSucceed = true,
                                StatusCode = 200,
                                Message = "Send Artwork Successfully"
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
