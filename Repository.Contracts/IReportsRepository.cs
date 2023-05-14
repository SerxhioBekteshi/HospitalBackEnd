using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IReportsRepository
    {
        void CreateRecord(Reports report);
        void DeleteRecord(Reports report);
        Task<Reports> GetRecordByIdAsync(int id);
        void UpdateRecord(Reports report);
        Task<IEnumerable<Reports>> GetAll();
        Task<Reports> GetRecordByReservationIdAsync(int reservationId);

    }
}
