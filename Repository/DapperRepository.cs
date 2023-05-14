using Dapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Entities.Models;
using Repository.Contracts;
using Repository.Extensions.Utility;
using Shared.DTO;
using Shared.RequestFeatures;

namespace Repository;

public class DapperRepository : IDapperRepository
{
    private readonly DapperContext _context;

    public DapperRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<ApplicationMenuDTO> GetMenuById(int id)
    {
        var query = @"
                SELECT *
            FROM ( 
	            SELECT 
		            apm.Id
		            , apm.Title
                    , apm.[Path]
                    , apm.Icon
					, apm.[Order]
	            FROM ApplicationMenu apm
				LEFT JOIN MenuPermissions mp ON apm.Id=mp.MenuId
				WHERE apm.Id = @Id
            ) result
        ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QuerySingleOrDefaultAsync<ApplicationMenuDTO>(query, new { Id = id });
            return result;
        }
    }

    
    public async Task<int> GetPeopleServedNumber()
    {
        var query = @"
                SELECT 
		         COUNT(r.Id) AS PeopleServed
	            FROM Reservations r
				WHERE r.Status = 1
        ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QuerySingleOrDefaultAsync<int>(query);
            return result;
        }
    }
    public async Task<IEnumerable<ApplicationMenuDTO>> AllMenuList()
    {
        var query = @"
               SELECT *
            FROM ( SELECT 
		            apm.Id
		            , apm.Title
                    , apm.[Path]
                    , apm.Icon
					, apm.[Order]
	            FROM ApplicationMenu apm
			EXCEPT(
	            SELECT 
		            apm.Id
		            , apm.Title
                    , apm.[Path]
                    , apm.Icon
					, apm.[Order]
	            FROM ApplicationMenu apm
				LEFT JOIN MenuPermissions mp ON apm.Id=mp.MenuId
				WHERE apm.Id = mp.MenuId)
            ) result
        ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<ApplicationMenuDTO>(query);
            return result;
        }
    }

    public async Task<IEnumerable<ApplicationMenuDTO>> GetApplicationMenuAsync(string userRole, int userId)
    {
        var query = @"
            
            SELECT am.*
            FROM MenuPermissions mp
	            INNER JOIN ApplicationMenu am ON mp.MenuId = am.Id
	            INNER JOIN [Permissions] p ON mp.PermissionId = p.Id
	            LEFT JOIN UserPermissions up ON mp.PermissionId = up.PermissionId
            WHERE p.IsActive = 1 AND up.IsActive = 1 AND up.UserId = @userId AND am.ParentId IS NULL AND p.SubjectId IS NULL

			UNION ALL

			SELECT am.*
            FROM MenuPermissions mp
	            INNER JOIN ApplicationMenu am ON mp.MenuId = am.Id
	            INNER JOIN [Permissions] p ON mp.PermissionId = p.Id
	            LEFT JOIN RolePermissions rp ON mp.PermissionId = rp.PermissionId
	            LEFT JOIN AspNetRoles r ON r.Id = rp.RoleId
            WHERE p.IsActive = 1 AND rp.IsActive = 1 AND r.Name = @userRole AND am.ParentId IS NULL AND p.SubjectId IS NULL 
					and rp.PermissionId NOT IN( SELECT up.PermissionId
												FROM MenuPermissions mp
													INNER JOIN ApplicationMenu am ON mp.MenuId = am.Id
													INNER JOIN [Permissions] p ON mp.PermissionId = p.Id
													LEFT JOIN UserPermissions up ON mp.PermissionId = up.PermissionId
												WHERE p.IsActive = 1 AND up.IsActive = 1 AND up.UserId = 74 AND am.ParentId IS NULL AND p.SubjectId IS NULL)

			ORDER BY am.[Order] ASC
        ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<ApplicationMenuDTO>(query, new { userRole, userId });
            return result;
        }
    }

    public async Task<IEnumerable<ApplicationMenuDTO>> GetChidlrenMenuAsync(string userRole, int userId, int parentId)
    {
        var query = @"
            SELECT am.*
            FROM MenuPermissions mp
	            INNER JOIN ApplicationMenu am ON mp.MenuId = am.Id
	            INNER JOIN [Permissions] p ON mp.PermissionId = p.Id
	            LEFT JOIN RolePermissions rp ON mp.PermissionId = rp.PermissionId
	            LEFT JOIN AspNetRoles r ON r.Id = rp.RoleId
            WHERE p.IsActive = 1 AND rp.IsActive = 1 AND r.Name = @userRole AND am.ParentId = @parentId

            UNION ALL

            SELECT am.*
            FROM MenuPermissions mp
	            INNER JOIN ApplicationMenu am ON mp.MenuId = am.Id
	            INNER JOIN [Permissions] p ON mp.PermissionId = p.Id
	            LEFT JOIN UserPermissions up ON mp.PermissionId = up.PermissionId
            WHERE p.IsActive = 1 AND up.IsActive = 1 AND up.UserId = @userId AND am.ParentId = @parentId
            ORDER BY am.[Order] ASC
        ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<ApplicationMenuDTO>(query, new { userRole, userId, parentId });
            return result;
        }

    }


    public async Task<IEnumerable<WorkingHourServiceDTO>> GetAllWorkingHoursServicesAsync(LookupRepositoryDTO filter)
    {
        var query = @"
             SELECT *
            FROM (
	            SELECT 
		            whs.Id
		            , whs.StartTime
                    , whs.EndTime 
                    , whs.Status
                    , whs.ServiceId
                    , s.Name AS ServiceName
                    , st.FirstName AS StaffName
                    , whs.StaffId
		            , whs.DateCreated
		            , whs.CreatedBy
		            , CONCAT(uc.FirstName, ' ', uc.LastName) AS CreatedByFullName
		            , whs.DateModified
		            , whs.ModifiedBy
		            , CONCAT(um.FirstName, '', um.LastName) AS ModifiedByFullName
	            FROM WorkingHourService whs 
		            INNER JOIN AspNetUsers uc ON whs.CreatedBy = uc.Id
		            LEFT JOIN AspNetUsers um ON whs.ModifiedBy = um.Id
					INNER JOIN AspNetUsers st on st.Id = whs.StaffId
                    INNER JOIN Services s on s.Id = whs.ServiceId
                WHERE whs.StartTime IS NOT NULL
            ) result
        ";

        //string lookupFilterNormalized = QueryBuilder.NormalizeLookUpFilter(filter.Filters, filter.FromSearchAll);
        //if (!string.IsNullOrWhiteSpace(lookupFilterNormalized))
        //    query += $" WHERE {lookupFilterNormalized}";

        //string lookupSortNormalized = QueryBuilder.NormalizeLookUpOrderBy(filter.Sorting);
        //if (!string.IsNullOrWhiteSpace(lookupSortNormalized))
        //    query += $" ORDER BY {lookupSortNormalized}";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<WorkingHourServiceDTO>(query);
            return result;
        }
    }

    public async Task<IEnumerable<DelayCountDTO>> DelaysTable(LookupRepositoryDTO filter)
    {
        var query = @"
              SELECT d.UserId as StaffId, CONCAT(uc.FirstName, ' ', uc.LastName) AS StaffName, COUNT(d.UserId) AS Count
			  FROM AspNetUsers uc 
		            INNER JOIN Delays d ON  d.UserId = uc.Id
			GROUP BY d.UserId,uc.FirstName, uc.LastName
        ";

        string lookupFilterNormalized = QueryBuilder.NormalizeLookUpFilter(filter.Filters, filter.FromSearchAll);
        if (!string.IsNullOrWhiteSpace(lookupFilterNormalized))
            query += $" WHERE {lookupFilterNormalized}";

        string lookupSortNormalized = QueryBuilder.NormalizeLookUpOrderBy(filter.Sorting);
        if (!string.IsNullOrWhiteSpace(lookupSortNormalized))
            query += $" ORDER BY {lookupSortNormalized}";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<DelayCountDTO>(query);
            return result;
        }
    }




    public async Task<PagedList<DelayCountDTO>> GetAllDelaysTable(LookupRepositoryDTO filter)
    {
        var result = await DelaysTable(filter);
        return PagedList<DelayCountDTO>.ToPagedList(result, filter.PageNumber, filter.PageSize);


    }
    public async Task<PagedList<WorkingHourServiceTableDTO>> SearchAvailableTimes(LookupRepositoryDTO filter)
    {
        var result = await AvailableTimesTable(filter);
        return PagedList<WorkingHourServiceTableDTO>.ToPagedList(result, filter.PageNumber, filter.PageSize);

    }
    public async Task<PagedList<ReservationTableDTO>> SearchReservations(LookupRepositoryDTO filter)
    {
        var result = await ReservationsTable(filter);
        return PagedList<ReservationTableDTO>.ToPagedList(result, filter.PageNumber, filter.PageSize);
    }

    public async Task<PagedList<ReservationTableDTO>> SearchPostponedReservations(LookupRepositoryDTO filter)
    {
        var result = await PostponedReservationsTable(filter);
        return PagedList<ReservationTableDTO>.ToPagedList(result, filter.PageNumber, filter.PageSize);
    }
    
    public async Task<PagedList<ReservationTableDTO>> SearchSuccededReservations(LookupRepositoryDTO filter, int staffId)
    {
        var result = await SucceededReservationsTable(filter, staffId);
        return PagedList<ReservationTableDTO>.ToPagedList(result, filter.PageNumber, filter.PageSize);
    }
    public async Task<PagedList<ReservationTableDTO>> SearchReservationsForStaff(LookupRepositoryDTO filter, int staffId)
    {
        var result = await ReservationsTableForStaff(filter, staffId);
        return PagedList<ReservationTableDTO>.ToPagedList(result, filter.PageNumber, filter.PageSize);
    }

    public async Task<PagedList<WorkingHourServiceTableDTO>> SearchAllServiceTimesForStaff(LookupRepositoryDTO filter, int staffId)
    {
        var result = await ServiceTimesTableForStaff(filter, staffId);
        return PagedList<WorkingHourServiceTableDTO>.ToPagedList(result, filter.PageNumber, filter.PageSize);
    }


    public async Task<PagedList<DeviceDTO>> SearchDevices(LookupRepositoryDTO filter)
    {
        var result = await DeviceTable(filter);
        return PagedList<DeviceDTO>.ToPagedList(result, filter.PageNumber, filter.PageSize);
    }
    public async Task<PagedList<ServiceDTO>> SearchServices(LookupRepositoryDTO filter)
    {
        var result = await ServicesTable(filter);
        return PagedList<ServiceDTO>.ToPagedList(result, filter.PageNumber, filter.PageSize);
    }

    public async Task<PagedList<UserTableDTO>> SearchUsers(LookupRepositoryDTO filter)
    {
        var result = await UsersTable(filter);

        return PagedList<UserTableDTO>.ToPagedList(result, filter.PageNumber, filter.PageSize);
    }

    public async Task<PagedList<ReportsDeriviedDTO>> SearchReports(LookupRepositoryDTO filter)
    {
        var result = await ReportsTable(filter);

        return PagedList<ReportsDeriviedDTO>.ToPagedList(result, filter.PageNumber, filter.PageSize);
    }
    public async Task<IEnumerable<DeviceDTO>> DeviceTable(LookupRepositoryDTO filter)
    {
        var query = @"
              SELECT *
            FROM (
	            SELECT 
		            d.Id
		            , d.Name
                    , d.isActive
		            , d.DateCreated
		            , d.CreatedBy
		            , CONCAT(uc.FirstName, ' ', uc.LastName) AS CreatedByFullName
		            , d.DateModified
		            , d.ModifiedBy
		            , CONCAT(um.FirstName, '', um.LastName) AS ModifiedByFullName
	            FROM Devices d 
		            INNER JOIN AspNetUsers uc ON d.CreatedBy = uc.Id
		            LEFT JOIN AspNetUsers um ON d.ModifiedBy = um.Id
            ) result
        ";

        string lookupFilterNormalized = QueryBuilder.NormalizeLookUpFilter(filter.Filters, filter.FromSearchAll);
        if (!string.IsNullOrWhiteSpace(lookupFilterNormalized))
            query += $" WHERE {lookupFilterNormalized}";

        string lookupSortNormalized = QueryBuilder.NormalizeLookUpOrderBy(filter.Sorting);
        if (!string.IsNullOrWhiteSpace(lookupSortNormalized))
            query += $" ORDER BY {lookupSortNormalized}";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<DeviceDTO>(query);
            return result;
        }
    }

    public async Task<IEnumerable<WorkingHourServiceTableDTO>> GetAllTimesAvailableForServiceId(int serviceId)
    {
        var query = @"
              SELECT *
            FROM (
	            SELECT 
                    whs.Id 
		            , whs.StartTime
                    , whs.EndTime
	            FROM Services s 
		            INNER JOIN WorkingHourService whs ON whs.ServiceId = s.Id
                WHERE whs.Status = 0 AND whs.serviceId = @serviceId
            ) result
        ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<WorkingHourServiceTableDTO>(query, new {serviceId = serviceId});
            return result;
        }
    }


    public async Task<IEnumerable<WorkingHourServiceTableDTO>> GetAllTimesAvailableForReservationServiceId(int serviceId)
    {
        var query = @"
              SELECT *
            FROM (
	            SELECT 
                    whs.Id 
		            , whs.StartTime
                    , whs.EndTime
	            FROM Services s 
		            INNER JOIN WorkingHourService whs ON whs.ServiceId = s.Id
                WHERE s.isActive = 1  AND whs.serviceId = @serviceId AND whs.StartTime IS NOT NULL
            ) result
        ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<WorkingHourServiceTableDTO>(query, new { serviceId = serviceId });
            return result;
        }
    }



    public async Task<IEnumerable<WorkingHourServiceTableDTO>> AvailableTimesTable(LookupRepositoryDTO filter)
    {

        var query = @"
              SELECT *
            FROM (
	            SELECT 
		            s.Name as ServiceName
                    , whs.Id 
		            , whs.StartTime
                    , whs.EndTime
                    , whs.Status
	            FROM Services s 
		            INNER JOIN WorkingHourService whs ON whs.ServiceId = s.Id
                WHERE CAST(whs.StartTime AS Date) > CAST(CURRENT_TIMESTAMP AS Date) AND (whs.Status = 0 or whs.Status = 2) AND whs.StartTime IS NOT NULL
            ) result
        ";

        string lookupFilterNormalized = QueryBuilder.NormalizeLookUpFilter(filter.Filters, filter.FromSearchAll);
        if (!string.IsNullOrWhiteSpace(lookupFilterNormalized))
            query += $" WHERE {lookupFilterNormalized}";

        string lookupSortNormalized = QueryBuilder.NormalizeLookUpOrderBy(filter.Sorting);
        if (!string.IsNullOrWhiteSpace(lookupSortNormalized))
            query += $" ORDER BY {lookupSortNormalized}";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<WorkingHourServiceTableDTO>(query);
            return result;
        }
    }

    public async Task<IEnumerable<ReservationTableDTO>> ReservationsTable(LookupRepositoryDTO filter)
    {
        var query = @"
              SELECT *
            FROM (
	            SELECT 
		            s.Name as ServiceName
		            , whs.StartTime
                    , whs.EndTime
		            , r.ClientId
		            , r.Name as ClientName
		            , r.Email
		            , r.PhoneNumber
                    , r.Status
                    , r.Id
	            FROM Services s 
		            INNER JOIN WorkingHourService whs ON whs.ServiceId = s.Id
		            INNER JOIN Reservations r ON r.WorkingHourServiceId = whs.Id
                WHERE (r.Status = 0 OR r.Status = 2) AND whs.StartTime IS NOT NULL
            ) result
        ";

        string lookupFilterNormalized = QueryBuilder.NormalizeLookUpFilter(filter.Filters, filter.FromSearchAll);
        if (!string.IsNullOrWhiteSpace(lookupFilterNormalized))
            query += $" WHERE {lookupFilterNormalized}";

        string lookupSortNormalized = QueryBuilder.NormalizeLookUpOrderBy(filter.Sorting);
        if (!string.IsNullOrWhiteSpace(lookupSortNormalized))
            query += $" ORDER BY {lookupSortNormalized}";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<ReservationTableDTO>(query);
            return result;
        }
    }
    public async Task<IEnumerable<ReservationTableDTO>> PostponedReservationsTable(LookupRepositoryDTO filter)
    {
        var query = @"
              SELECT *
            FROM (
	            SELECT 
		            s.Name as ServiceName
		            , whs.StartTime
                    , whs.EndTime
		            , r.ClientId
		            , r.Name as ClientName
		            , r.Email
		            , r.PhoneNumber
                    , r.Status
                    , r.Id
	            FROM Services s 
		            INNER JOIN WorkingHourService whs ON whs.ServiceId = s.Id
		            INNER JOIN Reservations r ON r.WorkingHourServiceId = whs.Id
                WHERE r.Status = 2  AND whs.StartTime IS NULL
            ) result
        ";

        string lookupFilterNormalized = QueryBuilder.NormalizeLookUpFilter(filter.Filters, filter.FromSearchAll);
        if (!string.IsNullOrWhiteSpace(lookupFilterNormalized))
            query += $" WHERE {lookupFilterNormalized}";

        string lookupSortNormalized = QueryBuilder.NormalizeLookUpOrderBy(filter.Sorting);
        if (!string.IsNullOrWhiteSpace(lookupSortNormalized))
            query += $" ORDER BY {lookupSortNormalized}";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<ReservationTableDTO>(query);
            return result;
        }
    }
    
    public async Task<IEnumerable<ReservationTableDTO>> SucceededReservationsTable(LookupRepositoryDTO filter, int staffId)
    {
        var query = @"
              SELECT *
            FROM (
	            SELECT 
		            s.Name as ServiceName
		            , whs.StartTime
                    , whs.EndTime

		            , r.ClientId
		            , r.Name as ClientName
		            , r.Email
		            , r.PhoneNumber
                    , r.Status
                    , r.Id
	            FROM Services s 
		            INNER JOIN WorkingHourService whs ON whs.ServiceId = s.Id
		            INNER JOIN Reservations r ON r.WorkingHourServiceId = whs.Id
                WHERE r.Status = 1 AND whs.StartTime IS NOT NULL AND whs.StaffId = @staffId AND r.Result IS NULL
            ) result
        ";

        string lookupFilterNormalized = QueryBuilder.NormalizeLookUpFilter(filter.Filters, filter.FromSearchAll);
        if (!string.IsNullOrWhiteSpace(lookupFilterNormalized))
            query += $" WHERE {lookupFilterNormalized}";

        string lookupSortNormalized = QueryBuilder.NormalizeLookUpOrderBy(filter.Sorting);
        if (!string.IsNullOrWhiteSpace(lookupSortNormalized))
            query += $" ORDER BY {lookupSortNormalized}";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<ReservationTableDTO>(query, new { staffId = staffId} );
            return result;
        }
    }

    public async Task<ReservationTableDTO> GetReservationDetails(int reservationId)
    {
        var query = @"
              SELECT *
            FROM (
	            SELECT 
		            s.Name as ServiceName
                    , s.Id as ServiceId
		            , whs.StartTime
                    , whs.EndTime
                    , whs.Id as WorkingHourServiceId
		            , r.ClientId
		            , r.Name as ClientName
		            , r.Email
		            , r.PhoneNumber
                    , r.Status
                    , r.Id
	            FROM Services s 
		            INNER JOIN WorkingHourService whs ON whs.ServiceId = s.Id
		            INNER JOIN Reservations r ON r.WorkingHourServiceId = whs.Id    
                WHERE r.Id = @reservationId
            ) result
        ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QuerySingleOrDefaultAsync<ReservationTableDTO>(query, new { reservationId = reservationId });
            return result;
        }
    }

    public async Task<IEnumerable<WorkingHourServiceTableDTO>> ServiceTimesTableForStaff(LookupRepositoryDTO filter, int staffId)
    {
        var query = @"
            SELECT*
            FROM(
                SELECT

                    whs.Id
                    , whs.StartTime
                    , whs.EndTime
                    , whs.Status
                    , whs.ServiceId
                    , s.Name AS ServiceName
                    , st.FirstName AS StaffName
                    , whs.StaffId
                    , whs.DateCreated
                    , whs.CreatedBy
                    , CONCAT(uc.FirstName, ' ', uc.LastName) AS CreatedByFullName
                    , whs.DateModified
                    , whs.ModifiedBy
                    , CONCAT(um.FirstName, '', um.LastName) AS ModifiedByFullName
                    FROM WorkingHourService whs
                    INNER JOIN AspNetUsers uc ON whs.CreatedBy = uc.Id
                    LEFT JOIN AspNetUsers um ON whs.ModifiedBy = um.Id
                    INNER JOIN AspNetUsers st on st.Id = whs.StaffId
                    INNER JOIN Services s on s.Id = whs.ServiceId
                    WHERE  CAST(whs.StartTime AS Date) > CAST(CURRENT_TIMESTAMP AS Date) AND uc.Id = @staffId AND (whs.Status = 0 or whs.Status = 2)
            ) result
        ";


        string lookupFilterNormalized = QueryBuilder.NormalizeLookUpFilter(filter.Filters, filter.FromSearchAll);
        if (!string.IsNullOrWhiteSpace(lookupFilterNormalized))
            query += $" WHERE {lookupFilterNormalized}";

        string lookupSortNormalized = QueryBuilder.NormalizeLookUpOrderBy(filter.Sorting);
        if (!string.IsNullOrWhiteSpace(lookupSortNormalized))
            query += $" ORDER BY {lookupSortNormalized}";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<WorkingHourServiceTableDTO>(query, new { staffId = staffId });
            return result;
        }
    }
    public async Task<IEnumerable<ReservationTableDTO>> ReservationsTableForStaff(LookupRepositoryDTO filter, int staffId)
    {
        var query = @"
                SELECT *
            FROM (
	            SELECT 
		            s.Name as ServiceName
		            , whs.StartTime
                    , whs.EndTime
                    , whs.StaffId
		            , r.ClientId
		            , r.Name as ClientName
		            , r.Email
		            , r.PhoneNumber
                    , r.Status
                    , r.Id,
							            CONCAT(uc.FirstName, ' ', uc.LastName) AS CreatedByFullName
	            FROM Services s 
		            INNER JOIN WorkingHourService whs ON whs.ServiceId = s.Id
		            INNER JOIN Reservations r ON r.WorkingHourServiceId = whs.Id
                    INNER JOIN AspNetUsers uc ON uc.Id = whs.StaffId
					WHERE uc.Id = @staffId AND (r.Status = 0 OR r.Status = 2) AND whs.StartTime IS NOT NULL
            ) result
        ";

        string lookupFilterNormalized = QueryBuilder.NormalizeLookUpFilter(filter.Filters, filter.FromSearchAll);
        if (!string.IsNullOrWhiteSpace(lookupFilterNormalized))
            query += $" WHERE {lookupFilterNormalized}";

        string lookupSortNormalized = QueryBuilder.NormalizeLookUpOrderBy(filter.Sorting);
        if (!string.IsNullOrWhiteSpace(lookupSortNormalized))
            query += $" ORDER BY {lookupSortNormalized}";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<ReservationTableDTO>(query, new { staffId  = staffId});
            return result;
        }
    }
    public async Task<IEnumerable<ServiceDTO>> ServicesTable(LookupRepositoryDTO filter)
    {
        var query = @"
              SELECT *
            FROM (
	            SELECT 
		            d.Id
		            , d.Name
                    , d.isActive
		            , d.DateCreated
		            , d.CreatedBy
		            , CONCAT(uc.FirstName, ' ', uc.LastName) AS CreatedByFullName
		            , d.DateModified
		            , d.ModifiedBy
		            , CONCAT(um.FirstName, '', um.LastName) AS ModifiedByFullName
	            FROM Services d 
		            INNER JOIN AspNetUsers uc ON d.CreatedBy = uc.Id
		            LEFT JOIN AspNetUsers um ON d.ModifiedBy = um.Id
            ) result
        ";

        string lookupFilterNormalized = QueryBuilder.NormalizeLookUpFilter(filter.Filters, filter.FromSearchAll);
        if (!string.IsNullOrWhiteSpace(lookupFilterNormalized))
            query += $" WHERE {lookupFilterNormalized}";

        string lookupSortNormalized = QueryBuilder.NormalizeLookUpOrderBy(filter.Sorting);
        if (!string.IsNullOrWhiteSpace(lookupSortNormalized))
            query += $" ORDER BY {lookupSortNormalized}";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<ServiceDTO>(query);
            return result;
        }
    }

    public async Task<IEnumerable<UserTableDTO>> UsersTable(LookupRepositoryDTO filter)
    {
        var query = @"
            SELECT *
            FROM (
	            SELECT 
		            u.Id
		            , u.FirstName
		            , u.LastName
					, u.Email
					, u.PhoneNumber
					, u.Address
					, u.City
					, u.State
					, u.ZipCode
		            , u.Gender
					, u.Birthday
					, ur.RoleId
					, r.Name as Role
					, u.DateCreated
	            FROM AspNetUsers u 
					INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
					LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
                WHERE r.Id = 2 OR r.Id = 3
            ) result
        ";

        string lookupFilterNormalized = QueryBuilder.NormalizeLookUpFilter(filter.Filters, filter.FromSearchAll);
        if (!string.IsNullOrWhiteSpace(lookupFilterNormalized))
            query += $" WHERE {lookupFilterNormalized}";

        string lookupSortNormalized = QueryBuilder.NormalizeLookUpOrderBy(filter.Sorting);
        if (!string.IsNullOrWhiteSpace(lookupSortNormalized))
            query += $" ORDER BY {lookupSortNormalized}";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<UserTableDTO>(query);
            return result;
        }

    }

    public async Task<IEnumerable<ReportsDeriviedDTO>> ReportsTable(LookupRepositoryDTO filter)
    {
        var query = @"
            SELECT *
           FROM (
	              SELECT 
                     whs.Id AS WorkingHourServiceId
                    , whs.StartTime 
                    , whs.EndTime
                    , res.Id AS ReservationId
                    , res.ClientId
                    , res.Email
                    , res.Name 
                    , res.PhoneNumber
                    , res.Surname
                    , res.Status
		            , r.Id As ReportId
		            , r.StatusMessage
					, r.DateCreated
					, r.CreatedBy
		            , CONCAT(uc.FirstName, ' ', uc.LastName) AS CreatedByFullName
					, r.DateModified
					, r.ModifiedBy
		            , CONCAT(um.FirstName, '', um.LastName) AS ModifiedByFullName
	            FROM WorkingHourService whs 
					INNER JOIN Reservations res ON res.WorkingHourServiceId = whs.Id
					INNER JOIN Reports r ON r.ReservationId = res.Id
                    INNER JOIN AspNetUsers uc ON r.CreatedBy = uc.Id
		            LEFT JOIN AspNetUsers um ON r.ModifiedBy = um.Id
            ) result
        ";

        string lookupFilterNormalized = QueryBuilder.NormalizeLookUpFilter(filter.Filters, filter.FromSearchAll);
        if (!string.IsNullOrWhiteSpace(lookupFilterNormalized))
            query += $" WHERE {lookupFilterNormalized}";

        string lookupSortNormalized = QueryBuilder.NormalizeLookUpOrderBy(filter.Sorting);
        if (!string.IsNullOrWhiteSpace(lookupSortNormalized))
            query += $" ORDER BY {lookupSortNormalized}";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<ReportsDeriviedDTO>(query);
            return result;
        }

    }

    public async Task<IEnumerable<DeviceDTO>> GetAllDevices(LookupRepositoryDTO filter)
    {
        var query = @"
            SELECT *
            FROM (
	            SELECT 
		            d.Id
		            , d.Name
		            , d.DateCreated
		            , d.CreatedBy
		            , CONCAT(uc.FirstName, ' ', uc.LastName) AS CreatedByFullName
		            , d.DateModified
		            , d.ModifiedBy
		            , CONCAT(um.FirstName, '', um.LastName) AS ModifiedByFullName
	            FROM Devices d 
		            INNER JOIN AspNetUsers uc ON d.CreatedBy = uc.Id
		            LEFT JOIN AspNetUsers um ON d.ModifiedBy = um.Id
            ) result
        ";

        //string lookupFilterNormalized = QueryBuilder.NormalizeLookUpFilter(filter.Filters, filter.FromSearchAll);
        //if (!string.IsNullOrWhiteSpace(lookupFilterNormalized))
        //    query += $" WHERE {lookupFilterNormalized}";

        //string lookupSortNormalized = QueryBuilder.NormalizeLookUpOrderBy(filter.Sorting);
        //if (!string.IsNullOrWhiteSpace(lookupSortNormalized))
        //    query += $" ORDER BY {lookupSortNormalized}";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<DeviceDTO>(query);
            return result;
        }
    }


    //GET ALL SERVICCES AS NORMAL LIST 
    public async Task<IEnumerable<ServiceListOptionsDTO>> GetAllServicesAsync(int workerId)
    {
        var query = @"
            SELECT *
            FROM (
	            SELECT 
		            s.Id AS Value
		            , s.Name AS Label     
	            FROM Services s
                INNER JOIN ServiceStaff ss on ss.ServiceId = s.Id
                INNER JOIN AspNetUsers u on u.Id = ss.StaffId
            WHERE s.isActive = 1 AND u.Id = @workerId
            ) result
        ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<ServiceListOptionsDTO>(query, new { workerId = workerId});
            return result;
        }
    }


    //AUTCOMPLETE FOR SERVICES
    public async Task<IEnumerable<ServiceListOptionsDTO>> GetAllServices(AutocompleteDTO autocomplete)
    {
        var top = autocomplete.Top;
        var query = @"
                  SELECT  TOP (@top) s.Id AS Value, s.Name AS Label
                 FROM Services s
                 WHERE s.isActive = 1
             ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<ServiceListOptionsDTO>(query, new { top });
            var searchTerm = autocomplete.SearchTerm;

            if (searchTerm is not null)
            {
                searchTerm = searchTerm.Trim().ToLower();
                result = result.Where(e => e.label.ToLower().Contains(searchTerm));

            }

            return result;

        }
    }


    //GET SERVICES IDS FOR DEVICE
    //public async Task<IEnumerable<int>> GetServicesIdsForDeviceId(int deviceId)
    //{
    //    var query = @"SELECT *
	   //             FROM(
		  //             SELECT 
				//			s.Id
		  //             FROM Services s
				//		INNER JOIN ServiceDevice sd ON sd.DeviceId = s.Id
    //                    WHERE sd.DeviceId = @deviceId
    //                    GROUP BY s.Id
    //                 )result";

    //    using (var connection = _context.CreateConnection())
    //    {
    //        var result = await connection.QueryAsync<int>(query, new { deviceId });
    //        return result.ToList();
    //    }
    //}


    //GET DEVICES AUTOCOMPLETE
    public async Task<IEnumerable<DeviceListOptionsDTO>> GetAllDevices(AutocompleteDTO autocomplete)
    {
        var top = autocomplete.Top;
        var query = @"
                  SELECT  TOP (@top) d.Id AS Value, d.Name AS Label
                 FROM Devices d
                 WHERE d.isActive = 1
             ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<DeviceListOptionsDTO>(query, new { top });
            var searchTerm = autocomplete.SearchTerm;

            if (searchTerm is not null)
            {
                searchTerm = searchTerm.Trim().ToLower();
                result = result.Where(e => e.label.ToLower().Contains(searchTerm));

            }

            return result;

        }
    }
    
    public async Task<IEnumerable<StaffListOptionDTO>> GetAllStaff(AutocompleteDTO autocomplete)
    {
        var top = autocomplete.Top;
        var query = @"
                 SELECT  TOP (@top) d.Id AS Value, d.FirstName AS Label
                 FROM AspNetUsers d
                 INNER JOIN AspNetUserRoles ar on ar.UserId = d.Id 
                 WHERE ar.RoleId = 3
             ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<StaffListOptionDTO>(query, new { top });
            var searchTerm = autocomplete.SearchTerm;

            if (searchTerm is not null)
            {
                searchTerm = searchTerm.Trim().ToLower();
                result = result.Where(e => e.label.ToLower().Contains(searchTerm));

            }

            return result;

        }
    }
    public async Task<IEnumerable<DeviceListDTO>> GetDevicesForServiceId(int serviceId)
    {
        var query = @"SELECT *
	                FROM(
		               SELECT 
							d.Id
							, d.Name
		               FROM Devices d
						INNER JOIN ServiceDevice sd ON d.Id = sd.DeviceId
                        WHERE d.isActive = 1 AND d.Id = @serviceId
                        GROUP BY d.Id, d.Name
                     )result";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<DeviceListDTO>(query, new { serviceId });
            return result;
        }
    }

    //public async Task<IEnumerable<int>> GetDevicesIdsForServiceId(int serviceId)
    //{
    //    var query = @"SELECT *
	   //             FROM(
		  //             SELECT 
				//			d.Id
		  //             FROM Devices d
				//		INNER JOIN ServiceDevice sd ON sd.DeviceId = d.Id
    //                    WHERE sd.ServiceId = @serviceId
    //                    GROUP BY d.Id
    //                 )result";

    //    using (var connection = _context.CreateConnection())
    //    {
    //        var result = await connection.QueryAsync<int>(query, new { serviceId });
    //        return result;
    //    }
    //}


    public async Task<IEnumerable<StaffListDTO>> GetStaffForServiceId(int serviceId)
    {
        var query = @"SELECT *
	                FROM(
		               SELECT 
							d.Id
							, d.Name
		               FROM Devices d
						INNER JOIN ServiceStaff sd ON d.Id = sd.UserId
                        WHERE d.isActive = 1 AND d.Id = @serviceId
                        GROUP BY d.Id, d.Name
                     )result";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<StaffListDTO>(query, new { serviceId });
            return result;
        }
    }

    public async Task<IEnumerable<ServiceListDTO>> GetServicesForDeviceId(int deviceId)
    {
        var query = @"SELECT *
	                FROM(
		               SELECT 
							s.Id
							, s.Name
		               FROM Services s
						INNER JOIN ServiceDevice sd ON s.Id = sd.DeviceId
                        WHERE s.isActive = 1 AND s.Id = @deviceId
                        GROUP BY s.Id, s.Name
                     )result";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<ServiceListDTO>(query, new { deviceId });
            return result;
        }
    }

    public async Task<IEnumerable<ServiceListDTO>> GetServicesForStaffId(int staffId)
    {
        var query = @"SELECT *
	                FROM(
		               SELECT 
							s.Id
							, s.Name
		               FROM Services s
						INNER JOIN ServiceStaff ss ON s.Id = ss.UserId
                        WHERE s.isActive = 1 AND s.Id = @staffId
                        GROUP BY s.Id, s.Name
                     )result";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<ServiceListDTO>(query, new { staffId });
            return result;
        }
    }


    public async Task<IEnumerable<UserTableDTO>> GetAllStaff(LookupRepositoryDTO filter)
    {
        var query = @"
           SELECT *
            FROM (
	            SELECT 
		            u.Id
		            , u.FirstName
		            , u.LastName
					, u.Email
					, u.PhoneNumber
					, u.Address
					, u.City
					, u.State
					, u.ZipCode
		            , u.Gender
					, u.Birthday
					, ur.RoleId
					, r.Name as Role
					, u.DateCreated
	            FROM AspNetUsers u 
					INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
					LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
                WHERE r.Id = 2 OR r.Id = 3
            ) result
        ";

        //string lookupFilterNormalized = QueryBuilder.NormalizeLookUpFilter(filter.Filters, filter.FromSearchAll);
        //if (!string.IsNullOrWhiteSpace(lookupFilterNormalized))
        //    query += $" WHERE {lookupFilterNormalized}";

        //string lookupSortNormalized = QueryBuilder.NormalizeLookUpOrderBy(filter.Sorting);
        //if (!string.IsNullOrWhiteSpace(lookupSortNormalized))
        //    query += $" ORDER BY {lookupSortNormalized}";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<UserTableDTO>(query);
            return result;
        }
    }

    public async Task<IEnumerable<WhsrDTO>> GetExistingReservationsWithService(int serviceId)
    {
        var query = @"
            SELECT *
            FROM (
	            SELECT 
		            s.Id AS ServiceId
					, s.Name AS ServiceName
					, whs.Id AS WorkingHourServiceId
					, r.Id AS ReservationId
	            FROM Services s 
					INNER JOIN WorkingHourService whs ON whs.ServiceId = s.Id
					INNER JOIN Reservations r ON r.WorkingHourServiceId = whs.Id
                WHERE s.Id = @serviceId
            ) result
        ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<WhsrDTO>(query, new { serviceId = serviceId });
            var rr = result.ToList();
            return rr;
        }
    }

    public async Task<IEnumerable<DwhrsDTO>> GetExistingReservationsWithDevice(int deviceId)
    {
        var query = @"
            SELECT *
            FROM (
	            SELECT 
		            d.Id AS DeviceId
					, d.Name AS DeviceName
					, whs.Id AS WorkingHourServiceId
                    , s.Id AS ServiceId
                    , s.Name AS ServiceName
					, r.Id AS ReservationId
	            FROM Devices d 
                    INNER JOIN ServiceDevice ds on ds.DeviceId = d.Id
                    INNER JOIN Services s on s.Id = ds.ServiceId
					INNER JOIN WorkingHourService whs ON whs.ServiceId = s.Id
					INNER JOIN Reservations r ON r.WorkingHourServiceId = whs.Id
                WHERE d.Id = @deviceId
            ) result
        ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<DwhrsDTO>(query, new { deviceId = deviceId });
            var rr = result.ToList();
            return rr;
        }
    }

    public async Task<IEnumerable<WhsrDTO>> GetExistingReservationsWithUser(int userId)
    {
        var query = @"
            SELECT *
            FROM (
	            SELECT 
					, whs.Id AS WorkingHourServiceId
					, r.Id AS ReservationId
	            FROM WorkingHourService whs 
					INNER JOIN Reservations r ON r.WorkingHourServiceId = whs.Id
                WHERE whs.staffId = @userId
            ) result
        ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<WhsrDTO>(query, new { userId = userId });
            var rr = result.ToList();
            return rr;
        }
    }

    public async Task<bool> DeactivateDevices(int[] deviceIds)
    {
        var query = @"UPDATE Devices
                    SET IsActive=0, DateModified=GETUTCDATE()
                    WHERE Id in @DeviceIds";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QuerySingleOrDefaultAsync<bool>(query, new { deviceIds });
            return result;
        }
    }

    public async Task<bool> ActivateDevices(int[] deviceIds)
    {
        var query = @"UPDATE Devices
                    SET IsActive=1, DateModified=GETUTCDATE()
                    WHERE Id in @DeviceIds";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QuerySingleOrDefaultAsync<bool>(query, new { deviceIds });
            return result;
        }
    }

    public async Task<bool> DeactivateServices(int[] serviceIds)
    {
        var query = @"UPDATE Services
                    SET IsActive=0, DateModified=GETUTCDATE()
                    WHERE Id in @ServiceIds";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QuerySingleOrDefaultAsync<bool>(query, new { serviceIds });
            return result;
        }
    }

    public async Task<bool> ActivateServices(int[] serviceIds)
    {
        var query = @"UPDATE Services
                    SET IsActive=1, DateModified=GETUTCDATE()
                    WHERE Id in @ServiceIds";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QuerySingleOrDefaultAsync<bool>(query, new { serviceIds });
            return result;
        }
    }

    public async Task<IEnumerable<UserTableDTO>> GetWorkers()
    {
        var query = @"
            SELECT *
            FROM (
	            SELECT 
		            u.Id
		            , u.FirstName
		            , u.LastName
	            FROM AspNetUsers u 
					INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
					LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
                WHERE r.Id = 3
            ) result
        ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<UserTableDTO>(query);
            return result;
        }

    }


    public async Task<List<WorkingHourServiceTableDTO>> GetTimesForServiceAndStaff (int serviceId, int staffId)
    {

        var query = @"
               SELECT *
            FROM (
	            SELECT 
		            whs.Id
		            , whs.StartTime
                    , whs.EndTime 
                    , whs.Status
                    , whs.ServiceId
                    , s.Name AS ServiceName
                    , st.FirstName AS StaffName
                    , whs.StaffId
	            FROM WorkingHourService whs 
		            INNER JOIN AspNetUsers uc ON whs.CreatedBy = uc.Id
		            LEFT JOIN AspNetUsers um ON whs.ModifiedBy = um.Id
					INNER JOIN AspNetUsers st on st.Id = whs.StaffId
                    INNER JOIN Services s on s.Id = whs.ServiceId
                WHERE whs.StartTime IS NOT NULL AND whs.ServiceId = @serviceId and whs.StaffId = @staffId AND (whs.Status = 0 OR whs.Status = 2)
            ) result 
        ";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<WorkingHourServiceTableDTO>(query, new { serviceId = serviceId, staffId = staffId});
            return result.ToList();
        }
    }
}