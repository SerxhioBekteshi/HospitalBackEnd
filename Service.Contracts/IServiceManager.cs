namespace Service.Contracts;

public interface IServiceManager
{
    IApplicationMenuService ApplicationMenuService { get; }
    //IEmailTemplateService EmailTemplateService { get; }
    IAuthenticationService AuthenticationService { get; }
    IDeviceService DeviceService { get; }
    IServiceService ServiceService { get; }
    IReservationService ReservationService { get; }
    IDeviceServiceService DeviceServiceService { get; }
    IUserService UserService { get; }
    IServiceStaffService ServiceStaffService { get; }
    IWorkingHourServiceService WorkingHourServiceService { get; }
    IReportsService ReportsService { get; }
    IDelayService DelayService { get; }

}