using be_artwork_sharing_platform.Core.Constancs;
using be_artwork_sharing_platform.Core.Dtos.RequestOrder;
using be_artwork_sharing_platform.Core.Interfaces;
using be_project_swp.Core.Dtos.RequestOrder;
using be_project_swp.Core.Dtos.Response;
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

        [HttpGet]
        [Route("get-by-id")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> GetRequestOrderById(long id)
        {
            try
            {
                var result = await _requestOrderService.GetRequestById(id);
                if (result == null)
                    return NotFound("Not found Request");
                return Ok(result);
            }
            catch
            {
                return BadRequest("Get Request Failed");
            }
        }

        [HttpPost]
        [Route("send-request")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> SendRequestOrder(string nick_Name, SendRequest sendRequest)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                string nickNameResquest = await _authService.GetCurrentNickName(userName);
                var ressult = await _requestOrderService.SendRequesrOrder(sendRequest, userId, nickNameResquest, nick_Name, userName);
                return StatusCode(ressult.StatusCode, ressult.Message);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
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
                string nickName = await _authService.GetCurrentNickName(userName);
                var result = _requestOrderService.GetMineRequestByNickName(nickName);
                return Ok(result);
            }
            catch
            {
                return BadRequest("Something wrong");
            }
        }

        [HttpGet]
        [Route("get-mine-request-by-id")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> GetRequestOfMinesById(long id)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                var result = await _requestOrderService.GetMineRequestById(id, userId);
                if(result == null)
                {
                    return NotFound();
                }
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
                string nickName = await _authService.GetCurrentNickName(userName);
                var result = _requestOrderService.GetMineOrderByNickName(nickName);
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
        public async Task<ActionResult<GeneralServiceResponseDto>> CancelRequest(long id)
        {
            try
            {
                var cancelRequest = new CancelRequest();
                string userName = HttpContext.User.Identity.Name;
                string currentNickName = await _authService.GetCurrentNickName(userName);
                var result = await _requestOrderService.CancelRequestByReceivier(id, cancelRequest, currentNickName, userName);
                return StatusCode(result.StatusCode, result.Message);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("update-request")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<ActionResult<GeneralServiceResponseDto>> AcceptRequest(long id)
        {
            try
            {
                var updateRequest = new UpdateRequest();
                string userName = HttpContext.User.Identity.Name;
                string currentNickName = await _authService.GetCurrentNickName(userName);
                var result = await _requestOrderService.AcceptRquest(id, updateRequest, currentNickName, userName);
                return StatusCode(result.StatusCode, result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("update-status-request")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<ActionResult<GeneralServiceResponseDto>> UpdateStatusRequest(long id, UpdateStatusRequest updateStatusRequest)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                var nickName = await _authService.GetCurrentNickName(userName);
                var result = await _requestOrderService.UpdateStatusRequest(id, nickName, updateStatusRequest, userName);
                return StatusCode(result.StatusCode, result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete-request")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> DeleteRequest(long id)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                bool checkActiveRequest = await _requestOrderService.GetActiveRequestByUserNameRequest(id, userId);
                if(checkActiveRequest)
                {
                    return BadRequest("Your request has been confirmed by the receiver or is in progress so do not delete this request!!!!!");
                }
                else
                {
                    var result = await _requestOrderService.DeleteRequestBySender(id, userId, userName);
                    if (result == 0) return NotFound("Request Not Found");
                    return Ok("Delete Request Successfully");
                }
            }
            catch
            {
                return BadRequest("Delete Request Failed");
            }
        }

        [HttpPatch]
        [Route("send-result-artwork")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<ActionResult<GeneralServiceResponseDto>> SendResultRequest(SendResultRequest sendResultRequest, long id)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string nickName = await _authService.GetCurrentNickName(userName);
                var result = await _requestOrderService.SendResultRequest(sendResultRequest, id, nickName, userName);
                return StatusCode(result.StatusCode, result.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
