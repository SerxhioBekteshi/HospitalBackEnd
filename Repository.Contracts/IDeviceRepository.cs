using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IDeviceRepository
    {
        void CreateRecord(Device device);
        Task<Device> GetRecordByIdAsync(int id);
        void UpdateRecord(Device device);
        void DeleteRecord(Device device);
        Task<IEnumerable<Device>> GetAll();

    }
}
