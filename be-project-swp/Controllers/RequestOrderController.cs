using be_artwork_sharing_platform.Core.Constancs;
using be_artwork_sharing_platform.Core.Dtos.General;
using be_artwork_sharing_platform.Core.Dtos.RequestOrder;
using be_artwork_sharing_platform.Core.Interfaces;
using be_project_swp.Core.Dtos.RequestOrder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace be_artwork_sharing_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestOrderController : ControllerBase
    {
        private readonly IRequestOrderService _requestOrderService;
        private readonly ILogService _logService;
        private readonly IAuthService _authService;

        public RequestOrderController(IRequestOrderService requestOrderService, ILogService logService, IAuthService authService)
        {
            _requestOrderService = requestOrderService;
            _logService = logService;
            _authService = authService;
        }

        [HttpPost]
        [Route("send-request")]
        [Authorize]
        public async Task<IActionResult> SendRequestOrder(SendRequest sendRequest, string user_Id)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                string fullNameResquest = await _authService.GetCurrentFullName(userName);
                string fullNameReceivier = await _authService.GetCurrentFullNameByUserId(user_Id);
                if(userId == user_Id)
                {
                    return BadRequest(new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 400,
                        Message = "You can not request you"
                    });
                }
                else
                {
                    _logService.SaveNewLog(userId, "Send Request Order");
                    _requestOrderService.SendRequesrOrder(sendRequest, userName, user_Id, fullNameResquest, fullNameReceivier);
                    return Ok(new GeneralServiceResponseDto()
                    {
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Send Request to Order Artwork Successfully"
                    });
                }
            }
            catch
            {
                return BadRequest("Send Request Failed");
            }
        }

        [HttpGet]
        [Route("get-mine-request")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> GetRequestOfMines()
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var result = _requestOrderService.GetMineRequestByUserName(userName);
                return Ok(result);
            }
            catch
            {
                return BadRequest("Something wrong");
            }
        }

        [HttpGet]
        [Route("get-mine-order")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> GetOrderOfMines()
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                var result = _requestOrderService.GetMineOrderByUserId(userId);
                return Ok(result);
            }
            catch
            {
                return BadRequest("Something wrong");
            }
        }

        [HttpPatch]
        [Route("cancel-request")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> CancelRequest(long id)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                var checkStatusRequest = await _requestOrderService.GetStatusRequestByUserNameRequest(id);
                if(checkStatusRequest == true)
                {
                    return BadRequest("You cannot cancel your request because your request is being fulfilled by the creator");
                }
                else
                {
                    await _requestOrderService.CancelRequest(id);
                    return Ok("Cancel Request Successfully");
                }
            }
            catch
            {
                return BadRequest("Something went wrong");
            }
        }

        [HttpPut]
        [Route("update-request")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> UpdatelRequest(long id, UpdateRequest updateRequest)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                _requestOrderService.UpdateRquest(id, updateRequest);
                return Ok("Update Request Successfully");
            }
            catch
            {
                return BadRequest("Something went wrong");
            }
        }
    }
}
