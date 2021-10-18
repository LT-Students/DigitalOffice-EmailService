using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplateText;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EmailService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IPatchDbEmailTemplateTextMapper
  {
    JsonPatchDocument<DbEmailTemplateText> Map(
      JsonPatchDocument<EditEmailTemplateTextRequest> request);
  }
}
