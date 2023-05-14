using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IDelayRepository
    {
        public void CreateRecord(Delay delay);
        public void DeleteRecord(Delay delay);
        public Task<Delay> GetRecordByIdAsync(int id);
        public Task<IEnumerable<Delay>> GetAll();
    }
}
