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
    public interface IReportsService
    {
        Task<PagedListResponse<IEnumerable<ReportsDeriviedDTO>>> GetAllReportsTable(LookupRepositoryDTO filter);
    }
}
