using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IWorkingHourServiceRepository
    {
        public void CreateRecord(WorkingHourService workingHourService);
        public void DeleteRecord(WorkingHourService workingHourService);
        public Task<WorkingHourService> GetRecordByIdAsync(int id);
        public void UpdateRecord(WorkingHourService workingHourService);
    }
}
