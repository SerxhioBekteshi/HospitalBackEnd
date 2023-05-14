using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository;

public class EmailTemplateRepository : RepositoryBase<EmailTemplate>, IEmailTemplateRepository
{
    public EmailTemplateRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }
    public void CreateRecord(EmailTemplate emailTemplate) => Create(emailTemplate);
    public void DeleteRecord(EmailTemplate emailTemplate) => Delete(emailTemplate);
    public async Task<EmailTemplate> GetRecordByCodeAsync(string code) =>
        await FindByCondition(c => c.Code.Equals(code)).SingleOrDefaultAsync();
    public async Task<EmailTemplate> GetRecordByIdAsync(int id) =>
        await FindByCondition(c => c.Id.Equals(id))
          .SingleOrDefaultAsync();
    public void UpdateRecord(EmailTemplate emailTemplate) => Update(emailTemplate);
}