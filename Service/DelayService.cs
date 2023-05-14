using AutoMapper;
using Entities.Exceptions;
using Entities.Models;
using Repository.Contracts;
using Service.Contracts;
using Shared.DTC;
using Shared.DTO;
using Shared.RequestFeatures;
using Shared.ResponseFeatures;
using Shared.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DelayService : IDelayService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IDapperRepository _dapperRepository;

        public DelayService(ILoggerManager logger, IMapper mapper, IRepositoryManager repositoryManager, IDapperRepository dapperRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _repositoryManager = repositoryManager;
            _dapperRepository = dapperRepository;
        }

        public async Task<PagedListResponse<IEnumerable<DelayCountDTO>>> GetAllDelaysTable(LookupRepositoryDTO filter)
        {
            try
            {
                var delayWithMetaData = await _dapperRepository.GetAllDelaysTable(filter);
                var columns = GetDataTableColumns();

                PagedListResponse<IEnumerable<DelayCountDTO>> response = new PagedListResponse<IEnumerable<DelayCountDTO>>
                {
                    TotalCount = delayWithMetaData.MetaData.TotalCount,
                    CurrentPage = delayWithMetaData.MetaData.CurrentPage,
                    PageSize = delayWithMetaData.MetaData.PageSize,
                    Columns = columns,
                    Rows = delayWithMetaData,
                    HasNext = delayWithMetaData.MetaData.HasNext,
                    HasPrevious = delayWithMetaData.MetaData.HasPrevious
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetAllDelaysTable), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        #region PrivateMethods
        private List<DataTableColumn> GetDataTableColumns()
        {
            // get the columns
            var columns = GenerateDataTableColumn<DelayColumn>.GetDataTableColumns();

            // return all columns
            return columns;
        }

        #endregion
    }
}
