using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IReservationRepository
    {
        public void CreateRecord(Reservation reservation);
        public void DeleteRecord(Reservation reservation);
        public Task<Reservation> GetRecordByIdAsync(int id);
        public void UpdateRecord(Reservation reservation);
        public Task<Reservation> GetRecordByWorkingIdAsync(int workingHourServiceId);

    }
}
