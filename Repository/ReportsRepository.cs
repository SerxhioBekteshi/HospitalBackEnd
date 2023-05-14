using DocumentFormat.OpenXml.Bibliography;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ReportsRepository : RepositoryBase<Reports>, IReportsRepository
    {
        public ReportsRepository(RepositoryContext repositoryContext)
          : base(repositoryContext)
        {
        }
        public void CreateRecord(Reports report) => Create(report);
        public void DeleteRecord(Reports report) => Delete(report);
        public async Task<Reports> GetRecordByIdAsync(int id) =>
        await FindByCondition(c => c.Id.Equals(id))
              .SingleOrDefaultAsync();


        public async Task<Reports> GetRecordByReservationIdAsync(int reservationId) =>
       await FindByCondition(c => c.ReservationId.Equals(reservationId))
             .SingleOrDefaultAsync();
        public void UpdateRecord(Reports report) => Update(report);
        public async Task<IEnumerable<Reports>> GetAll() => await FindAll().ToListAsync();

    }
}
