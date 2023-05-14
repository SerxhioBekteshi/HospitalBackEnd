using AutoMapper;
using Entities.Exceptions;
using Entities.Models;
using Repository.Contracts;
using Service.Contracts;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceStaffService : IServiceStaffService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IDapperRepository _dapperRepository;

        public ServiceStaffService(ILoggerManager logger, IMapper mapper, IRepositoryManager repositoryManager, IDapperRepository dapperRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _repositoryManager = repositoryManager;
            _dapperRepository = dapperRepository;
        }


        public async Task<bool> CreateRecord(ServiceStaffDTO createServiceStaffDTO, int userId)
        {
            try
            {
                var serviceStaff = _mapper.Map<ServiceStaff>(createServiceStaffDTO);
                serviceStaff.DateCreated = DateTime.UtcNow;
                serviceStaff.CreatedBy = userId;

                _repositoryManager.ServiceStaffRepository.CreateRecord(serviceStaff);
                await _repositoryManager.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(CreateRecord), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<bool> DeleteRecord(int id)
        {
            try
            {
                var existingServiceStaff = await GetServiceStaffAndCheckIfExistsAsync(id);

                _repositoryManager.ServiceStaffRepository.DeleteRecord(existingServiceStaff);
                await _repositoryManager.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(DeleteRecord), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<bool> UpdateRecord(ServiceStaffDTO updateServiceStaffDTO, int id, int userId)
        {
            try
            {
                var existingServiceStaff = await GetServiceStaffAndCheckIfExistsAsync(id);

                _mapper.Map(updateServiceStaffDTO, existingServiceStaff);

                existingServiceStaff.DateModified = DateTime.UtcNow;
                existingServiceStaff.ModifiedBy = userId;

                _repositoryManager.ServiceStaffRepository.UpdateRecord(existingServiceStaff);
                await _repositoryManager.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(UpdateRecord), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        #region Private Methods

        private async Task<ServiceStaff> GetServiceStaffAndCheckIfExistsAsync(int id)
        {
            var existingServiceStaff = await _repositoryManager.ServiceStaffRepository.GetRecordByIdAsync(id);
            if (existingServiceStaff is null)
                throw new NotFoundException(string.Format("Lidhja me Id: {0} ndërmjet sherbimit dhe perdoruesit nuk u gjet!", id));

            return existingServiceStaff;
        }

        #endregion
    }
}
