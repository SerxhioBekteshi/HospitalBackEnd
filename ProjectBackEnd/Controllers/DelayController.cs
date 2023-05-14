using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using ProjectBackEnd.Authorization;
using ProjectBackEnd.Utility;
using Service.Contracts;
using Shared.DTO;
using Shared.RequestFeatures;
using Shared.ResponseFeatures;

namespace ProjectBackEnd.Controllers
{


    [Route("api/delay")]
    [ApiController]
    [CustomAuthorize]
    public class DelayController : ControllerBase
    {
        private readonly IServiceManager _service;

        public DelayController(IServiceManager service)
        {
            _service = service;
        }

        [HttpPost("get-all")]
        public async Task<IActionResult> GetAllCounts([FromBody] LookupRepositoryDTO filter)
        {
            var result = await _service.DelayService.GetAllDelaysTable(filter);
            var baseResponse = new BaseResponse<PagedListResponse<IEnumerable<DelayCountDTO>>, object>
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
