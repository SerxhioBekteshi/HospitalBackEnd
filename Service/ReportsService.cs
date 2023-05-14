using AutoMapper;
using Entities.Exceptions;
using Repository.Contracts;
using Shared.DTC;
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
    public class ReportsService : IReportsService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IDapperRepository _dapperRepository;

        public ReportsService(ILoggerManager logger, IMapper mapper, IRepositoryManager repositoryManager, IDapperRepository dapperRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _repositoryManager = repositoryManager;
            _dapperRepository = dapperRepository;
        }
        public async Task<PagedListResponse<IEnumerable<ReportsDeriviedDTO>>> GetAllReportsTable(LookupRepositoryDTO filter)
        {
            try
            {
                var reportsWithMetaData = await _dapperRepository.SearchReports(filter);
                var columns = GetDataTableColumns();

                PagedListResponse<IEnumerable<ReportsDeriviedDTO>> response = new PagedListResponse<IEnumerable<ReportsDeriviedDTO>>
                {
                    TotalCount = reportsWithMetaData.MetaData.TotalCount,
                    CurrentPage = reportsWithMetaData.MetaData.CurrentPage,
                    PageSize = reportsWithMetaData.MetaData.PageSize,
                    Columns = columns,
                    Rows = reportsWithMetaData,
                    HasNext = reportsWithMetaData.MetaData.HasNext,
                    HasPrevious = reportsWithMetaData.MetaData.HasPrevious
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetAllReportsTable), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        private List<DataTableColumn> GetDataTableColumns()
        {
            // get the columns
            var columns = GenerateDataTableColumn<ReportsColumn>.GetDataTableColumns();

            // return all columns
            return columns;
        }
    }
}
