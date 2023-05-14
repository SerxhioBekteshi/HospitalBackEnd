
using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ProjectBackEnd.Utility;
using Repository.Contracts;
using Service.Contracts;
using Shared.DTO;
using Shared.ResponseFeatures;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationMenuController : ControllerBase
    {
        private readonly IServiceManager _service;
        public ApplicationMenuController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("get-all")]

        public async Task<IActionResult> GetMenu()
        {
            var menu = await _service.ApplicationMenuService.GetMenu(ClaimsUtility.ReadCurrentUserRole2(User.Claims));
            var baseResponse = new BaseResponse<IEnumerable<ApplicationMenuDTO>, object>
            {
                Result = true,
                Data = menu,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);

        }
    }
}
