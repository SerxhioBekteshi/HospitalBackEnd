using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public class ApplicationMenuRepository : RepositoryBase<ApplicationMenu>, IApplicationMenuRepository
{
    public ApplicationMenuRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }
    public async Task<IEnumerable<ApplicationMenu>> GetMenu(int RoleId) =>
            FindByCondition(c => c.RoleId.Equals(RoleId)).ToList();
}
