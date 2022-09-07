using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Contracts.Persistence;
using User.Domain.Entities;
using User.Infrastructure.Persistence;

namespace User.Infrastructure.Repositories
{
    public class EmailRepository : RepositoryBase<EmailTemplate>, IEmailRepository
    {

        public EmailRepository(UserContext dbContext) : base(dbContext)
        {
        }
        public async Task<EmailTemplate> GetTemplateByType(string type)
        {
            var template = await _dbContext.EmailTemplates
               .Where(e => e.TemplateType.Type == type && e.IsActive)
               .FirstOrDefaultAsync();

            return template;
        }
    }
}
