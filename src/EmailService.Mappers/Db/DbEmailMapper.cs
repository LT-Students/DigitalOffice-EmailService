using System;
using LT.DigitalOffice.EmailService.Mappers.Db.Email.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Email;

namespace LT.DigitalOffice.EmailService.Mappers.Db.Email
{
  public class DbEmailMapper : IDbEmailMapper
  {
    public DbEmail Map(
      ISendEmailRequest request)
    {
      if (request is null)
      {
        return null;
      }

      return new DbEmail
      {
        Id = Guid.NewGuid(),
        SenderId = request.SenderId,
        Receiver = request.Receiver,
        CreatedAtUtc = DateTime.UtcNow
      };
    }
  }
}
