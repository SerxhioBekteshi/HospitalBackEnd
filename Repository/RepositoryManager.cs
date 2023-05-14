using Repository.Contracts;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;
    private readonly Lazy<IEmailTemplateRepository> _emailTemplateRepository;
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<IApplicationMenuRepository> _applicationMenuRepository;
    private readonly Lazy<IDeviceRepository> _deviceRepository;
    private readonly Lazy<IServiceRepository> _serviceRepository;
    private readonly Lazy<IReservationRepository> _reservationRepository;
    private readonly Lazy<IDeviceServiceRepository> _deviceServiceRepository;
    private readonly Lazy<IServiceStaffRepository> _serviceStaffRepository;
    private readonly Lazy<IWorkingHourServiceRepository> _workingHourServiceRepository;
    private readonly Lazy<IReportsRepository> _reportsRepository;
    private readonly Lazy<IDelayRepository> _delayRepository;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        _emailTemplateRepository = new Lazy<IEmailTemplateRepository>(() => new EmailTemplateRepository(repositoryContext));
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(repositoryContext));
        _applicationMenuRepository = new Lazy<IApplicationMenuRepository>(() => new ApplicationMenuRepository(repositoryContext));
        _deviceRepository = new Lazy<IDeviceRepository>(() => new DeviceRepository(repositoryContext));
        _serviceRepository = new Lazy<IServiceRepository>(() => new ServiceRepository(repositoryContext));
        _reservationRepository = new Lazy<IReservationRepository>(() => new ReservationRepository(repositoryContext));
        _deviceServiceRepository = new Lazy<IDeviceServiceRepository>(() => new DeviceServiceRepository(repositoryContext));
        _serviceStaffRepository = new Lazy<IServiceStaffRepository>(() => new ServiceStaffRepository(repositoryContext));
        _workingHourServiceRepository = new Lazy<IWorkingHourServiceRepository>(() => new WorkingHourServiceRepository(repositoryContext));
        _reportsRepository = new Lazy<IReportsRepository>(() => new ReportsRepository(repositoryContext));
        _delayRepository = new Lazy<IDelayRepository>(() => new DelayRepository(repositoryContext));

    }

    public IEmailTemplateRepository EmailTemplateRepository => _emailTemplateRepository.Value;
    public IUserRepository UserRepository => _userRepository.Value;
    public IApplicationMenuRepository ApplicationMenuRepository => _applicationMenuRepository.Value;
    public IDeviceRepository DeviceRepository => _deviceRepository.Value;
    public IServiceRepository ServiceRepository => _serviceRepository.Value;
    public IReservationRepository ReservationRepository => _reservationRepository.Value;
    public IDeviceServiceRepository DeviceServiceRepository => _deviceServiceRepository.Value;
    public IServiceStaffRepository ServiceStaffRepository => _serviceStaffRepository.Value;
    public IWorkingHourServiceRepository WorkingHourServiceRepository => _workingHourServiceRepository.Value;
    public IReportsRepository ReportsRepository => _reportsRepository.Value;
    public IDelayRepository DelayRepository => _delayRepository.Value;

    public async Task SaveAsync()
    {
        _repositoryContext.ChangeTracker.AutoDetectChangesEnabled = false;
        await _repositoryContext.SaveChangesAsync();
    }
}