
using AutoMapper;
using Cryptography;
using EmailService;
using Entities.Configuration;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Repository.Contracts;
using Service.Contracts;
using Shared.Utility;

namespace Service;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IApplicationMenuService> _applicationMenuService;
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<IDeviceService> _deviceService;
    private readonly Lazy<IServiceService> _serviceService;
    private readonly Lazy<IReservationService> _reservationService;
    private readonly Lazy<IDeviceServiceService> _deviceServiceService;
    private readonly Lazy<IServiceStaffService> _serviceStaffService;
    private readonly Lazy<IWorkingHourServiceService> _workingHourServiceService;
    //private readonly Lazy<IEmailTemplateService> _emailTemplateService;
    private readonly Lazy<IUserService> _userService; //do marri ato qe merr authenticationService
    private readonly Lazy<IReportsService> _reportsService; //do marri ato qe merr authenticationService
    private readonly Lazy<IDelayService> _delayService; //do marri ato qe merr authenticationService
    private readonly IEmailSender _emailSender;

    public ServiceManager(IRepositoryManager repositoryManager
        , IDapperRepository dapperRepository
        , ILoggerManager logger
        , IMapper mapper
        , UserManager<ApplicationUser> userManager
        , IOptions<JwtConfiguration> configuration
        , IEmailSender emailSender
        , DefaultConfiguration defaultConfig
        , SignInManager<ApplicationUser> signInManager
        , ICryptoUtils cryptoUtils
        , IEmailSender _emailSender
        )
    {
        _applicationMenuService = new Lazy<IApplicationMenuService>(() => new ApplicationMenuService(dapperRepository, logger, repositoryManager, mapper)); 
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(logger, mapper, userManager, configuration, repositoryManager, signInManager, emailSender, cryptoUtils, defaultConfig));
        _userService = new Lazy<IUserService>(() => new UserService(logger, mapper, repositoryManager, userManager, dapperRepository, defaultConfig, emailSender));
        _deviceService = new Lazy<IDeviceService>(() => new DeviceService(logger, mapper, repositoryManager, dapperRepository, emailSender));
        _serviceService = new Lazy<IServiceService>(() => new ServiceService(logger, mapper, repositoryManager, dapperRepository, emailSender));
        _reservationService= new Lazy<IReservationService>(() => new ReservationService(logger, mapper, repositoryManager, dapperRepository, emailSender));
        _deviceServiceService = new Lazy<IDeviceServiceService>(() => new DeviceServiceService(logger, mapper, repositoryManager, dapperRepository));
        _serviceStaffService = new Lazy<IServiceStaffService>(() => new ServiceStaffService(logger, mapper, repositoryManager, dapperRepository));
        _workingHourServiceService = new Lazy<IWorkingHourServiceService>(() => new WorkingHourServiceService(logger, mapper, repositoryManager, dapperRepository));
        _reportsService = new Lazy<IReportsService>(() => new ReportsService(logger, mapper, repositoryManager, dapperRepository));
        _delayService = new Lazy<IDelayService>(() => new DelayService(logger, mapper, repositoryManager, dapperRepository));

    }

    public IApplicationMenuService ApplicationMenuService => _applicationMenuService.Value;
    public IAuthenticationService AuthenticationService => _authenticationService.Value;
    public IDeviceService DeviceService => _deviceService.Value;
    public IServiceService ServiceService => _serviceService.Value;
    public IReservationService ReservationService => _reservationService.Value;
    public IDeviceServiceService DeviceServiceService => _deviceServiceService.Value;
    public IUserService UserService => _userService.Value;
    public IServiceStaffService ServiceStaffService => _serviceStaffService.Value;
    public IWorkingHourServiceService WorkingHourServiceService => _workingHourServiceService.Value;
    public IReportsService ReportsService => _reportsService.Value;
    public IDelayService DelayService => _delayService.Value;

}