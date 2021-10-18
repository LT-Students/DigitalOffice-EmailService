using System;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EmailService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbEmailTemplateTextMapper
  {
    DbEmailTemplateText Map(EmailTemplateTextRequest request, Guid? emailTemplateId = null);
  }
}
