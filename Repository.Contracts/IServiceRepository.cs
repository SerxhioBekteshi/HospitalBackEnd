using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IServiceRepository
    {
        void CreateRecord(Services service);
        Task<Services> GetRecordByIdAsync(int id);
        void UpdateRecord(Services service);
        void DeleteRecord(Services service);
        Task<IEnumerable<Services>> GetAllRecordsAsync();
    }
}
