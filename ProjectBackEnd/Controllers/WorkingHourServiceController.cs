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

    [Route("api/workingHourServices")]
    [ApiController]
    [CustomAuthorize]
    public class WorkingHourServiceController : ControllerBase
    {
        private readonly IServiceManager _service;

        public WorkingHourServiceController(IServiceManager service)
        {
            _service = service;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateWorkingHourService([FromBody] WorkingHourServiceDTO createWorkingHourService)
        {
            if (ClaimsUtility.ReadCurrentUserRole(User.Claims) == UserRole.Staff)
            {
                var result = await _service.WorkingHourServiceService.CreateRecord(createWorkingHourService, ClaimsUtility.ReadCurrentUserId(User.Claims));
                var baseResponse = new BaseResponse<object, object>
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
                var result = await _service.WorkingHourServiceService.CreateRecord(createWorkingHourService, (int)createWorkingHourService.workerId);

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var result = await _service.WorkingHourServiceService.GetRecordById(id);
            var baseResponse = new BaseResponse<WorkingHourServiceDTO, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] WorkingHourServiceDTO workingHourServiceDTO)
        {

            var result = await _service.WorkingHourServiceService.UpdateRecord(workingHourServiceDTO, id, ClaimsUtility.ReadCurrentUserId(User.Claims));

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
        public async Task<IActionResult> DeleteService(int id)
        {
            var result = await _service.WorkingHourServiceService.DeleteRecord(id);
            var baseResponse = new BaseResponse<object, object>
            {
                Result = result,
                Data = "",
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        [HttpPost("get-all")]
        public async Task<IActionResult> GetAllWorkingHours([FromBody] LookupRepositoryDTO filter)
        {
            PagedListResponse<IEnumerable<WorkingHourServiceTableDTO>> result;
            if (ClaimsUtility.ReadCurrentUserRole(User.Claims) == UserRole.Recepsionist)
                result = await _service.WorkingHourServiceService.GetAllWorkingHoursServicesAsync(filter);
            else
                result = await _service.WorkingHourServiceService.GetAllTimeServicesTableForStaff(filter, ClaimsUtility.ReadCurrentUserId(User.Claims));

            var baseResponse = new BaseResponse<PagedListResponse<IEnumerable<WorkingHourServiceTableDTO>>, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        [HttpGet("{id}/timesforservice")]
        public async Task<IActionResult> GetTimesForServiceId(int id)
        {
            var result = await _service.WorkingHourServiceService.GetTimesForServiceId(id);

            var baseResponse = new BaseResponse<IEnumerable<WorkingHourServiceTableDTO>, object>
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
