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

    [Route("api/device")]
    [ApiController]
    [CustomAuthorize]
    public class DeviceController : ControllerBase
    {
        private readonly IServiceManager _service;

        public DeviceController(IServiceManager service)
        {
            _service = service;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateDevice([FromBody] DeviceDTO createDevice)
        {

                var result = await _service.DeviceService.CreateRecord(createDevice, ClaimsUtility.ReadCurrentUserId(User.Claims));
                var baseResponse = new BaseResponse<object, object>
                {
                    Result = result,
                    Data = result,
                    Errors = "",
                    StatusCode = StatusCodes.Status200OK
                };
                return Ok(baseResponse);
           
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeviceById(int id)
        {
            var result = await _service.DeviceService.GetRecordById(id);
            var baseResponse = new BaseResponse<DeviceDTO, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DeviceDTO updatePages)
        {    
                var result = await _service.DeviceService.UpdateRecord(updatePages, id, ClaimsUtility.ReadCurrentUserId(User.Claims));
                var baseResponse = new BaseResponse<object, object>
                {
                    Result = result,
                    Data = "",
                    Errors = "",
                    StatusCode = StatusCodes.Status200OK
                };
                return Ok(baseResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var result = await _service.DeviceService.DeleteRecord(id, ClaimsUtility.ReadCurrentUserId(User.Claims));
            var baseResponse = new BaseResponse<object, object>
            {
                Result = result,
                Data = "",
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        //[HttpPost("get-all")]
        //public async Task<IActionResult> GetAllDevices([FromBody] LookupRepositoryDTO filter)
        //{
        //    var result = await _service.DeviceService.GetAllDevicesAsync(filter);
        //    var baseResponse = new BaseResponse<IEnumerable<DeviceDTO>, object>
        //    {
        //        Result = true,
        //        Data = result,
        //        Errors = "",
        //        StatusCode = StatusCodes.Status200OK
        //    };
        //    return Ok(baseResponse);
        //}

        [HttpPost("get-all")]
        public async Task<IActionResult> GetAllDevices([FromBody] LookupRepositoryDTO filter)
        {
            var result = await _service.DeviceService.GetAllDevicesTable(filter);
            var baseResponse = new BaseResponse<PagedListResponse<IEnumerable<DeviceDTO>>, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }


        [HttpPut("{deviceId}/add-services")]
        public async Task<IActionResult> PostServicesToDevice(int deviceId, [FromBody] PostServicesToDeviceDTO postServicesToDeviceDTO)
        {
            var result = await _service.DeviceService.PostServicesToDevice(deviceId, postServicesToDeviceDTO, ClaimsUtility.ReadCurrentUserId(User.Claims));
            var baseResponse = new BaseResponse<object, object>
            {
                Result = result,
                Data = "",
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }


        [HttpPost("deactivate")]
        public async Task<IActionResult> DeactivateDevices(int[] deviceIds)
        {
            var result = await _service.DeviceService.DeactivateDeviceAsync(deviceIds, ClaimsUtility.ReadCurrentUserId(User.Claims));
            var baseResponse = new BaseResponse<object, object>
            {
                Result = result,
                Data = "",
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }
        [HttpPost("activate")]
        public async Task<IActionResult> ActivateDevice(int[] deviceIds)
        {
            var result = await _service.DeviceService.ActivateDeviceAsync(deviceIds);
            var baseResponse = new BaseResponse<object, object>
            {
                Result = result,
                Data = "",
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }
    }
}
