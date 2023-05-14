
using Shared.DTO;
using Shared.RequestFeatures;
using Shared.ResponseFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IDelayService
    {
        public Task<PagedListResponse<IEnumerable<DelayCountDTO>>> GetAllDelaysTable (LookupRepositoryDTO filter);

    }
}
