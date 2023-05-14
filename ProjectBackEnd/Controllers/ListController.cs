using Entities.Models;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectBackEnd.Authorization;
using ProjectBackEnd.Utility;
using Service.Contracts;
using Shared.DTO;
using Shared.RequestFeatures;
using Shared.ResponseFeatures;
using Shared.Utility;

namespace ProjectBackEnd.Controllers
{

    [Route("api/list/")]
    [ApiController]
    [CustomAuthorize]
    public class ListController : ControllerBase
    {
        private readonly IServiceManager _service;

        public ListController(IServiceManager service)
        {
            _service = service;
        }

      

        [HttpPost("services")]
        public async Task<IActionResult> GetDeviceById([FromBody] ServicesForUser? servicesForUser)
        {
            if (ClaimsUtility.ReadCurrentUserRole(User.Claims) == UserRole.Staff)
            {
                var result = await _service.ServiceService.GetAllServicesAsync(ClaimsUtility.ReadCurrentUserId(User.Claims));
                var baseResponse = new BaseResponse<IEnumerable<ServiceListOptionsDTO>, object>
                {
                    Result = true,
                    Data = result,
                    Errors = "",
                    StatusCode = StatusCodes.Status200OK
                };
                return Ok(baseResponse);

            }
            else
            {
                var result = await _service.ServiceService.GetAllServicesAsync(servicesForUser.WorkerId);
                var baseResponse = new BaseResponse<IEnumerable<ServiceListOptionsDTO>, object>
                {
                    Result = true,
                    Data = result,
                    Errors = "",
                    StatusCode = StatusCodes.Status200OK
                };
                return Ok(baseResponse);
            }    
        }

        [HttpPost("device-services")]
        public async Task<IActionResult> GetServices([FromBody] AutocompleteDTO autocomplete)
        {
            var result = await _service.DeviceService.GetAllServices(autocomplete);
            var baseResponse = new BaseResponse<IEnumerable<ServiceListOptionsDTO>, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        [HttpPost("service-devices")]
        public async Task<IActionResult> GetDevices([FromBody] AutocompleteDTO autocomplete)
        {
            var result = await _service.ServiceService.GetAllDevices(autocomplete);
            var baseResponse = new BaseResponse<IEnumerable<DeviceListOptionsDTO>, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }


        [HttpPost("service-staff")]
        public async Task<IActionResult> GetStaff([FromBody] AutocompleteDTO autocomplete)
        {
            var result = await _service.ServiceService.GetAllStaff(autocomplete);
            var baseResponse = new BaseResponse<IEnumerable<StaffListOptionDTO>, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }
    }
}
