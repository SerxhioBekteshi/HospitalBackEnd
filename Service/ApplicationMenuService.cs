using AutoMapper;
using Entities.Exceptions;
using Entities.Models;
using Repository.Contracts;
using Service.Contracts;
using Shared.DTO;

namespace Service;

public class ApplicationMenuService : IApplicationMenuService
{
    private readonly IDapperRepository _dapperRepository;
    private readonly ILoggerManager _logger;
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public ApplicationMenuService(IDapperRepository dapperRepository, ILoggerManager logger, IRepositoryManager repository, IMapper mapper)
    {
        _dapperRepository = dapperRepository;
        _logger = logger;
        _repository = repository;   
        _mapper = mapper;   
    }

    public async Task<List<ApplicationMenuDTO>> GetMenuByRoleAndUserId(string userRole, int userId)
    {
        List<ApplicationMenuDTO> result = new List<ApplicationMenuDTO>();

        var parentMenus = await _dapperRepository.GetApplicationMenuAsync(userRole, userId);

        if (parentMenus == null)
            throw new NotFoundException($"Menu nuk u gjet për rolin {userRole}");

        foreach (var menu in parentMenus)
        {
            var childrenMenu = await _dapperRepository.GetChidlrenMenuAsync(userRole, userId, menu.Id);
            if (childrenMenu != null && childrenMenu.Count() > 0)
                //menu.Children = childrenMenu.ToList();

            result.Add(menu);
        }

        return result;
    }

    public async Task<IEnumerable<ApplicationMenuDTO>> GetAllMenu()
    {
        try
        {
            var applicationMenu = await _dapperRepository.AllMenuList();
            if (applicationMenu is null)
                throw new NotFoundException("Nuk ka asnjë menu!");

            return applicationMenu;
        }
        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task<ApplicationMenuDTO> GetRecordById(int id)
    {
        try
        {
            var existingMenu = await _dapperRepository.GetMenuById(id);
            if (existingMenu is null)
                throw new NotFoundException($"Menu me Id:{id} nuk ekziston!");
            return existingMenu;
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(GetRecordById), ex.Message));
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task<IEnumerable<ApplicationMenuDTO>> GetMenu(int roleId)
    {
        try
        {
            var existingMenu = await _repository.ApplicationMenuRepository.GetMenu(roleId);
            var menuDto = _mapper.Map<IEnumerable<ApplicationMenuDTO>>(existingMenu);
    

            if (menuDto is null)
                throw new NotFoundException($"Menu nuk ekziston!");
            return menuDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(GetMenu), ex.Message));
            throw new BadRequestException(ex.Message);
        }
    }
}