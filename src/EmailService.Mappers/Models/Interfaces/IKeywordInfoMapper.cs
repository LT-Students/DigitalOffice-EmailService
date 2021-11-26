using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EmaileService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IKeywordInfoMapper
  {
    KeywordInfo Map(DbKeyword dbKeyword);
  }
}
