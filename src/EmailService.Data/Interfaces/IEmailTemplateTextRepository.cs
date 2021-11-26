using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EmailService.Data.Interfaces
{
  [AutoInject]
  public interface IEmailTemplateTextRepository
  {
    Task<Guid?> CreateAsync(DbEmailTemplateText request);

    Task<bool> EditAsync(Guid emailTemplateTextId, JsonPatchDocument<DbEmailTemplateText> patch);
  }
}
