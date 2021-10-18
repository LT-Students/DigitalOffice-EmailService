using LT.DigitalOffice.EmailService.Mappers.Models.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Models;

namespace LT.DigitalOffice.EmailService.Mappers.Models
{
  public class EmailTemplateTextInfoMapper : IEmailTemplateTextInfoMapper
  {
    public EmailTemplateTextInfo Map(DbEmailTemplateText dbEmailTemplateText)
    {
      if (dbEmailTemplateText == null)
      {
        return null;
      }

      return new EmailTemplateTextInfo
      {
        Id = dbEmailTemplateText.Id,
        Subject = dbEmailTemplateText.Subject,
        Text = dbEmailTemplateText.Text,
        Language = dbEmailTemplateText.Language
      };
    }
  }
}
