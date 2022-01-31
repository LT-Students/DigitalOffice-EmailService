using LT.DigitalOffice.EmailService.Mappers.Models.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Models;

namespace LT.DigitalOffice.EmailService.Mappers.Models
{
  public class EmailInfoMapper : IEmailInfoMapper
  {
    public EmailInfo Map(DbEmail dbEmail)
    {
      if (dbEmail == null)
      {
        return null;
      }

      return new EmailInfo
      {
        Id = dbEmail.Id,
        Body = dbEmail.Text,
        Subject = dbEmail.Subject,
        Receiver = dbEmail.Receiver
      };
    }
  }
}
