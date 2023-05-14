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

namespace ProjectBackEnd.Controllers;

[Route("api/user")]
[ApiController]
[CustomAuthorize]
public class UserController : ControllerBase
{
    private readonly IServiceManager _service;

    public UserController(IServiceManager service)
    {
        _service = service;
    }

    [HttpPost()]
    [Authorize(Roles = UserRole.Manager)]
    public async Task<IActionResult> CreateUser([FromBody] AddUserDTO addUserDto)
    {
        var result = await _service.UserService.AddUser(addUserDto, ClaimsUtility.ReadCurrentUserId(User.Claims));
        var baseResponse = new BaseResponse<object, object>
            {
                Result = true,
                Data = result,                  
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
        return Ok(baseResponse);
    }


    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetUserDetails(int id)
    {
        var result = await _service.UserService.GetUserById(id);
        var baseResponse = new BaseResponse<UserDetailsDTO, object>
        {
            Result = true,
            Data = result,
            Errors = "",
            StatusCode = StatusCodes.Status200OK
        };
        return Ok(baseResponse);
    }

    //[HttpPost("get-all")]
    //[Authorize(Roles = UserRole.Manager)]
    //public async Task<IActionResult> GetAllStaff([FromBody] LookupRepositoryDTO filter)
    //{
    //    var result = await _service.UserService.GetAllRecords(filter);
    //    var baseResponse = new BaseResponse<IEnumerable<UserTableDTO>, object>
    //    {
    //        Result = true,
    //        Data = result,
    //        Errors = "",
    //        StatusCode = StatusCodes.Status200OK
    //    };
    //    return Ok(baseResponse);
    //}


    [HttpPost("get-all")]
    public async Task<IActionResult> GetAllWorkers([FromBody] LookupRepositoryDTO filter)
    {
        var result = await _service.UserService.GetAllUsers(filter);
        var baseResponse = new BaseResponse<PagedListResponse<IEnumerable<UserTableDTO>>, object>
        {
            Result = true,
            Data = result,
            Errors = "",
            StatusCode = StatusCodes.Status200OK
        };
        return Ok(baseResponse);
    }

    [HttpGet("get-workers")]
    public async Task<IActionResult> GetWorkers()
    {
        var result = await _service.UserService.GetWorkers();
        var baseResponse = new BaseResponse<IEnumerable<UserTableDTO>, object>
        {
            Result = true,
            Data = result,
            Errors = "",
            StatusCode = StatusCodes.Status200OK
        };
        return Ok(baseResponse);
    }


    [HttpPut("{userId}/add-services")]
    public async Task<IActionResult> PostServicesToStaff(int userId, [FromBody] PostServicesToStaffDTO postServicesToStaffDTO)
    {
        var result = await _service.UserService.PostServicesToStaff(userId, postServicesToStaffDTO, ClaimsUtility.ReadCurrentUserId(User.Claims));
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
    public async Task<IActionResult> DeleteStaff(int id)
    {
        var result = await _service.UserService.DeleteRecord(id);
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
