using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EmailService.Data.Interfaces
{
  [AutoInject]
  public interface IEmailTemplateRepository
  {
    Task<Guid?> CreateAsync(DbEmailTemplate request);

    Task<bool> EditAsync(Guid emailTemplateId, JsonPatchDocument<DbEmailTemplate> request);

    Task<DbEmailTemplate> GetAsync(Guid emailTemplateId);

    Task<DbEmailTemplate> GetAsync(int type);

    Task<(List<DbEmailTemplate> dbEmailTempates, int totalCount)> FindAsync(FindEmailTemplateFilter filter);
  }
}
