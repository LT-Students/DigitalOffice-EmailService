using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.ParseEntity;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EmailService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbKeywordMapper
  {
    DbKeyword Map(CreateKeywordRequest request);
  }
}
