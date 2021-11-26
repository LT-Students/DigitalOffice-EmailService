using System;
using LT.DigitalOffice.EmailService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;

namespace LT.DigitalOffice.EmailService.Mappers.Db
{
  public class DbEmailTemplateTextMapper : IDbEmailTemplateTextMapper
  {
    public DbEmailTemplateText Map(EmailTemplateTextRequest request, Guid? emailTemplateId = null)
    {
      if (request == null)
      {
        return null;
      }

      return new DbEmailTemplateText
      {
        Id = Guid.NewGuid(),
        EmailTemplateId = emailTemplateId.HasValue ? emailTemplateId.Value : request.EmailTemplateId.Value,
        Subject = request.Subject,
        Text = request.Text,
        Language = request.Language
      };
    }
  }
}
