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

    [Route("api/reports")]
    [ApiController]
    [CustomAuthorize]
    public class ReportsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public ReportsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpPost("get-all")]
        public async Task<IActionResult> GetAllReports ([FromBody] LookupRepositoryDTO filter)
        {
            var result = await _service.ReportsService.GetAllReportsTable(filter);
            var baseResponse = new BaseResponse<PagedListResponse<IEnumerable<ReportsDeriviedDTO>>, object>
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
