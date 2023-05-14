using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Entities.Models;
using Shared.DTO;

namespace ProjectBackEnd;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Register
        CreateMap<UserRegisterDTO, ApplicationUser>();
        CreateMap<ApplicationUser, PostServicesToStaffDTO>().ReverseMap();

        //EmailTemplate
        CreateMap<EmailTemplate, EmailTemplateDTO>().ReverseMap();

        //Device
        CreateMap<DeviceDTO, Device>().ReverseMap();
        CreateMap<Device, PostServicesToDeviceDTO>().ReverseMap();

        //Service
        CreateMap<ServiceDTO, Services>().ReverseMap();
        CreateMap<Services, PostDevicesToServiceDTO>().ReverseMap();
        CreateMap<Services, PostStaffToServiceDTO>().ReverseMap();

        //DeviceService
        CreateMap<DeviceServiceDTO, ServiceDevice>().ReverseMap();

        //Reservation
        CreateMap<ReservationDTO, Reservation>().ReverseMap();

        //Reservation
        CreateMap<WorkingHourServiceDTO, WorkingHourService>().ReverseMap();

        //Company
        CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<DateTime?, DateTime>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<bool?, bool>().ConvertUsing((src, dest) => src ?? dest);

        //User
        CreateMap<ApplicationUser, UserListDTO>().ReverseMap();
        CreateMap<ApplicationUser, UserUpdateDTO>().ReverseMap();
        CreateMap<ApplicationUser, UserDetailsDTO>().ReverseMap();
        CreateMap<ApplicationUser, UserTableDTO>().ReverseMap();
        CreateMap<ApplicationUser, AddUserDTO>().ReverseMap();

        //Reports 
        CreateMap<Reports, ReportsDTO>().ReverseMap();

        //Menu
        CreateMap<ApplicationMenu, ApplicationMenuDTO>().ReverseMap();

        //Reports
        CreateMap<Reports, ReportsDeriviedDTO>().ReverseMap();


        //Delays 
        CreateMap<Delay, DelayDTO>().ReverseMap();
    }
}