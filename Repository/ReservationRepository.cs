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
    internal class ReservationRepository : RepositoryBase<Reservation>, IReservationRepository
    {
        public ReservationRepository(RepositoryContext repositoryContext)
       : base(repositoryContext)
        {
        }

        public void CreateRecord(Reservation reservation) => Create(reservation);
        public void DeleteRecord(Reservation reservation) => Delete(reservation);
        public async Task<Reservation> GetRecordByIdAsync(int id) =>
            await FindByCondition(c => c.Id.Equals(id))
            .SingleOrDefaultAsync();

        public async Task<Reservation> GetRecordByWorkingIdAsync(int workingHourServiceId) =>
         await FindByCondition(c => c.WorkingHourServiceId.Equals(workingHourServiceId))
         .SingleOrDefaultAsync();
        public void UpdateRecord(Reservation reservation) => Update(reservation);
    
    }
}
