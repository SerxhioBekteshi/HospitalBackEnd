using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;
using Shared.RequestFeatures;
using Shared.ResponseFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IReservationService
    {
        public Task<int> CreateRecord(ReservationDTO createReservationDTO, int userId);
        public Task<bool> DeleteRecord(int id);
        public Task<bool> UpdateRecord(ReservationDTO updateReservationDto, int id, int userId, string StatusMessage);
        public Task<bool> CancelReservation(int id, int userId);
        public Task<Reservation> GetReservationById(int id);
        public Task<ReservationTableDTO> GetReservationDetailsById(int id);
        public Task<PagedListResponse<IEnumerable<ReservationTableDTO>>> GetAllReservationTable(LookupRepositoryDTO filter);
        public Task<PagedListResponse<IEnumerable<ReservationTableDTO>>> GetAllPostponedReservations(LookupRepositoryDTO filter);
        public Task<PagedListResponse<IEnumerable<ReservationTableDTO>>> GetAllSuccededReservations(LookupRepositoryDTO filter, int staffId);
        public Task<PagedListResponse<IEnumerable<ReservationTableDTO>>> GetAllReservationTableForStaff(LookupRepositoryDTO filter, int staffId);
        public Task<bool> PostponeReservation(int id, postponeReservationDTO postponeReservation, int userId);
        public Task<bool> SucceedReservation(int id, int userId);
        public Task<bool> RegisterStartTimeReservation(int id, int userId);
        public Task<bool> SubmitResult(int id, SubmitResultDTO result);
        public Task<IEnumerable<WorkingHourServiceTableDTO>> AvailableTimesForReservation(int id);
        public Task<bool> PostponeReservationOfNoTime(int id, postponeReservationDTO postponeReservation, int userId);
        public Task<int> GetPeopleServedNumber();

    }
}
