using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EmailService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IPatchDbEmailTemplateMapper
  {
    JsonPatchDocument<DbEmailTemplate> Map(
      JsonPatchDocument<EditEmailTemplateRequest> request);
  }
}
