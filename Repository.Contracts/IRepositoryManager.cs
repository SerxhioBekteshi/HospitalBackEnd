namespace Repository.Contracts;

public interface IRepositoryManager
{
    IEmailTemplateRepository EmailTemplateRepository { get; }
    IUserRepository UserRepository { get; }
    IApplicationMenuRepository ApplicationMenuRepository { get; }
    IDeviceRepository DeviceRepository { get; }
    IServiceRepository ServiceRepository { get; }
    IReservationRepository ReservationRepository { get; }
    IDeviceServiceRepository DeviceServiceRepository { get; }
    IServiceStaffRepository ServiceStaffRepository { get; }
    IWorkingHourServiceRepository WorkingHourServiceRepository { get; }
    IReportsRepository ReportsRepository { get; }
    IDelayRepository DelayRepository { get; }

    Task SaveAsync();
}