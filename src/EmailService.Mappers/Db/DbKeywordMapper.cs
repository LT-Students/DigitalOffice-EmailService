using System;
using LT.DigitalOffice.EmailService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.ParseEntity;

namespace LT.DigitalOffice.EmailService.Mappers.Db
{
  public class DbKeywordMapper : IDbKeywordMapper
  {
    public DbKeyword Map(AddKeywordRequest request)
    {
      if (request == null)
      {
        return null;
      }

      return new DbKeyword
      {
        Id = Guid.NewGuid(),
        Keyword = request.Keyword,
        ServiceName = (int)request.ServiceName,
        EntityName = "Db" + request.EntityName,
        PropertyName = request.PropertyName
      };
    }
  }
}
