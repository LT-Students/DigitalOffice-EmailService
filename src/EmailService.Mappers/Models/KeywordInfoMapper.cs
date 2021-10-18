using System;
using LT.DigitalOffice.EmaileService.Mappers.Models.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto;
using LT.DigitalOffice.EmailService.Models.Dto.Enums;

namespace LT.DigitalOffice.EmailService.Mappers.Models
{
  public class KeywordInfoMapper : IKeywordInfoMapper
  {
    public KeywordInfo Map(DbKeyword dbKeyword)
    {
      if (dbKeyword == null)
      {
        return null;
      }

      return new KeywordInfo
      {
        Id = dbKeyword.Id,
        Keyword = dbKeyword.Keyword,
        ServiceName = (ServiceName)dbKeyword.ServiceName,
        EntityName = dbKeyword.EntityName.StartsWith("db", StringComparison.OrdinalIgnoreCase) ?
          dbKeyword.EntityName[2..] :
          dbKeyword.EntityName,
        PropertyName = dbKeyword.PropertyName
      };
    }
  }
}
