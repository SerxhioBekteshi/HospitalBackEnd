using AutoMapper;
using Entities.Exceptions;
using Entities.Models;
using Repository.Contracts;
using Service.Contracts;
using Shared.DTO;


namespace Service
{
    public class DeviceServiceService : IDeviceServiceService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IDapperRepository _dapperRepository;

        public DeviceServiceService(ILoggerManager logger, IMapper mapper, IRepositoryManager repositoryManager, IDapperRepository dapperRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _repositoryManager = repositoryManager;
            _dapperRepository = dapperRepository;
        }


        public async Task<bool> CreateRecord(DeviceServiceDTO createDeviceServiceDTO, int userId)
        {
            try
            {
                var deviceService = _mapper.Map<ServiceDevice>(createDeviceServiceDTO);
                deviceService.DateCreated = DateTime.UtcNow;
                deviceService.CreatedBy = userId;

                _repositoryManager.DeviceServiceRepository.CreateRecord(deviceService);
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
                var existingDeviceService = await GetDeviceServiceAndCheckIfExistsAsync(id);

                _repositoryManager.DeviceServiceRepository.DeleteRecord(existingDeviceService);
                await _repositoryManager.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(DeleteRecord), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<bool> UpdateRecord(DeviceServiceDTO updateDeviceServiceDTO, int id, int userId)
        {
            try
            {
                var existingDeviceService = await GetDeviceServiceAndCheckIfExistsAsync(id);

                _mapper.Map(updateDeviceServiceDTO, existingDeviceService);

                existingDeviceService.DateModified = DateTime.UtcNow;
                existingDeviceService.ModifiedBy = userId;

                _repositoryManager.DeviceServiceRepository.UpdateRecord(existingDeviceService);
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

        private async Task<ServiceDevice> GetDeviceServiceAndCheckIfExistsAsync(int id)
        {
            var existingDeviceService = await _repositoryManager.DeviceServiceRepository.GetRecordByIdAsync(id);
            if (existingDeviceService is null)
                throw new NotFoundException(string.Format("Lidhja me Id: {0} ndërmjet pajisjes dhe sherbimit nuk u gjet!", id));

            return existingDeviceService;
        }
        #endregion
    }
}