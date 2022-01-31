using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Requests.Email;

namespace LT.DigitalOffice.EmailService.Mappers.Db.Email.Interfaces
{
  [AutoInject]
  public interface IDbEmailMapper
  {
    DbEmail Map(ISendEmailRequest request);
  }
}
