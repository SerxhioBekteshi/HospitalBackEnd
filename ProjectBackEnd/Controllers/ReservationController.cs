using EmailService;
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

    [Route("api/reservation")]
    [ApiController]
    [CustomAuthorize]
    public class ReservationController : ControllerBase
    {
        private readonly IServiceManager _service;

        public ReservationController(IServiceManager service)
        {
            _service = service;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationDTO createReservation)
        {

            var result = await _service.ReservationService.CreateRecord(createReservation, ClaimsUtility.ReadCurrentUserId(User.Claims));
            var baseResponse = new BaseResponse<object, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);

        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelReservation(int id)
        {

            var result = await _service.ReservationService.CancelReservation(id, ClaimsUtility.ReadCurrentUserId(User.Claims));
            var baseResponse = new BaseResponse<object, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);

        }


        [HttpPut("{id}/succeed")]
        public async Task<IActionResult> SuccedReservation(int id)
        {

            var result = await _service.ReservationService.SucceedReservation(id, ClaimsUtility.ReadCurrentUserId(User.Claims));
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
        public async Task<IActionResult> GetReservationById(int id)
        {
            var result = await _service.ReservationService.GetReservationById(id);
            var baseResponse = new BaseResponse<Reservation, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetReservationDetailsById(int id)
        {
            var result = await _service.ReservationService.GetReservationDetailsById(id);
            var baseResponse = new BaseResponse<ReservationTableDTO, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        [HttpPut("{id}/postponeReservation")]
        public async Task<IActionResult> GetReservationDetailsById(int id, [FromBody] postponeReservationDTO postponeReservation)
        {
            var result = await _service.ReservationService.PostponeReservation(id, postponeReservation, ClaimsUtility.ReadCurrentUserId(User.Claims));
            var baseResponse = new BaseResponse<bool, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }
        [HttpPut("{id}/postponeReservationOfNoTime")]
        public async Task<IActionResult> PostponeReservation(int id, [FromBody] postponeReservationDTO postponeReservation)
        {
            var result = await _service.ReservationService.PostponeReservationOfNoTime(id, postponeReservation, ClaimsUtility.ReadCurrentUserId(User.Claims));
            var baseResponse = new BaseResponse<bool, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        [HttpPost("{id}/registerStartTime")]
        public async Task<IActionResult> RegisterStartTimeReservation(int id)
        {
            var result = await _service.ReservationService.RegisterStartTimeReservation(id, ClaimsUtility.ReadCurrentUserId(User.Claims));
            var baseResponse = new BaseResponse<bool, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ReservationDTO updateReservation, string? StatusMessage)
        {
            var result = await _service.ReservationService.UpdateRecord(updateReservation, id, ClaimsUtility.ReadCurrentUserId(User.Claims), StatusMessage);
            var baseResponse = new BaseResponse<object, object>
            {
                Result = result,
                Data = "",
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteReservation(int id)
        //{
        //    var result = await _service.ReservationService.DeleteRecord(id);
        //    var baseResponse = new BaseResponse<object, object>
        //    {
        //        Result = result,
        //        Data = "",
        //        Errors = "",
        //        StatusCode = StatusCodes.Status200OK
        //    };
        //    return Ok(baseResponse);
        //}

        [HttpPost("get-all")]
        public async Task<IActionResult> GetAllReservations([FromBody] LookupRepositoryDTO filter)
        {
            PagedListResponse<IEnumerable<ReservationTableDTO>> result;
            if (ClaimsUtility.ReadCurrentUserRole(User.Claims) == UserRole.Recepsionist)
                result = await _service.ReservationService.GetAllReservationTable(filter);
            else
                result = await _service.ReservationService.GetAllReservationTableForStaff(filter, ClaimsUtility.ReadCurrentUserId(User.Claims));

            var baseResponse = new BaseResponse<PagedListResponse<IEnumerable<ReservationTableDTO>>, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }


        [HttpPost("postponed/get-all")]
        public async Task<IActionResult> GetAllPostponedReservations([FromBody] LookupRepositoryDTO filter)
        {

            var result = await _service.ReservationService.GetAllPostponedReservations(filter);
            var baseResponse = new BaseResponse<PagedListResponse<IEnumerable<ReservationTableDTO>>, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }


        [HttpPost("succeeded/get-all")]
        public async Task<IActionResult> GetAllSucceededReservations([FromBody] LookupRepositoryDTO filter)
        {

            var result = await _service.ReservationService.GetAllSuccededReservations(filter, ClaimsUtility.ReadCurrentUserId(User.Claims));
            var baseResponse = new BaseResponse<PagedListResponse<IEnumerable<ReservationTableDTO>>, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        [HttpPut("{id}/submit-result")]
        public async Task<IActionResult> SubmitResult(int id, [FromBody] SubmitResultDTO sumbitResult)
        {

            var result = await _service.ReservationService.SubmitResult(id, sumbitResult);
            var baseResponse = new BaseResponse<bool, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        [HttpGet("{id}/availableTimes")]
        public async Task<IActionResult> AvailableTimes(int id)
        {

            var result = await _service.ReservationService.AvailableTimesForReservation(id);
            var baseResponse = new BaseResponse<IEnumerable<WorkingHourServiceTableDTO>, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        [HttpGet("done")]
        public async Task<IActionResult> DoneReservatios()
        {

            var result = await _service.ReservationService.GetPeopleServedNumber();
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
