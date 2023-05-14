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

    [Route("api/service")]
    [ApiController]
    [CustomAuthorize]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceManager _service;

        public ServiceController(IServiceManager service)
        {
            _service = service;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateService([FromBody] ServiceDTO createService)
        {

            var result = await _service.ServiceService.CreateRecord(createService, ClaimsUtility.ReadCurrentUserId(User.Claims));

            var baseResponse = new BaseResponse<object, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
      
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var result = await _service.ServiceService.GetRecordById(id);
            var baseResponse = new BaseResponse<ServiceDTO, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ServiceDTO updateService)
        {
           
            var result = await _service.ServiceService.UpdateRecord(updateService, id, ClaimsUtility.ReadCurrentUserId(User.Claims));

            var baseResponse = new BaseResponse<object, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var result = await _service.ServiceService.DeleteRecord(id, ClaimsUtility.ReadCurrentUserId(User.Claims));
            var baseResponse = new BaseResponse<object, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        //[HttpPost("get-all")]
        //public async Task<IActionResult> GetAllDevices([FromBody] LookupRepositoryDTO filter)
        //{
        //    var result = await _service.ServiceService.GetAllServicesAsync(filter);
        //    var baseResponse = new BaseResponse<IEnumerable<ServiceDTO>, object>
        //    {
        //        Result = true,
        //        Data = result,
        //        Errors = "",
        //        StatusCode = StatusCodes.Status200OK
        //    };
        //    return Ok(baseResponse);
        //}

        [HttpPost("get-all")]
        public async Task<IActionResult> GetAllServices([FromBody] LookupRepositoryDTO filter)
        {
            var result = await _service.ServiceService.GetAllServicesTable(filter);
            var baseResponse = new BaseResponse<PagedListResponse<IEnumerable<ServiceDTO>>, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        [HttpPut("{serviceId}/add-devices")]
        public async Task<IActionResult> PostDevicesToService(int serviceId, [FromBody] PostDevicesToServiceDTO postDevicesToServiceDTO)
        {
            var result = await _service.ServiceService.PostDevicesToService(serviceId, postDevicesToServiceDTO, ClaimsUtility.ReadCurrentUserId(User.Claims));
            var baseResponse = new BaseResponse<object, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        [HttpPut("{serviceId}/add-staff")]
        public async Task<IActionResult> PostStaffToService(int serviceId, [FromBody] PostStaffToServiceDTO postStaffToServiceDTO)
        {
            var result = await _service.ServiceService.PostStaffToServiceId(serviceId, postStaffToServiceDTO, ClaimsUtility.ReadCurrentUserId(User.Claims));
            var baseResponse = new BaseResponse<object, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }



        [HttpPost("deactivate")]
        public async Task<IActionResult> DeactivateServices(int[] serviceIds)
        {
            var result = await _service.ServiceService.DeactivateServiceAsync(serviceIds, ClaimsUtility.ReadCurrentUserId(User.Claims));
            var baseResponse = new BaseResponse<object, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }
        [HttpPost("activate")]
        public async Task<IActionResult> ActivateDevice(int[] serviceIds)
        {
            var result = await _service.ServiceService.ActivateServiceAsync(serviceIds);
            var baseResponse = new BaseResponse<object, object>
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
