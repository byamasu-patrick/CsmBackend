using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entities;

namespace User.Application.Contracts.Persistence
{
    public interface IEmailRepository : IAsyncRepository<EmailTemplate>
    {
        Task<EmailTemplate> GetTemplateByType(string type);
    }
}
