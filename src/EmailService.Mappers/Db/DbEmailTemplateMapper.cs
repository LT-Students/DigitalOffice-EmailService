using System;
using System.Linq;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.EmailService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EmailService.Mappers.Db
{
  public class DbEmailTemplateMapper : IDbEmailTemplateMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDbEmailTemplateTextMapper _dbEmailTemplateTextMapper;

    public DbEmailTemplateMapper(
      IHttpContextAccessor httpContextAccessor,
      IDbEmailTemplateTextMapper dbEmailTemplateTextMapper)
    {
      _httpContextAccessor = httpContextAccessor;
      _dbEmailTemplateTextMapper = dbEmailTemplateTextMapper;
    }

    public DbEmailTemplate Map(EmailTemplateRequest request)
    {
      if (request == null)
      {
        return null;
      }

      var templateId = Guid.NewGuid();

      return new DbEmailTemplate
      {
        Id = templateId,
        Name = request.Name,
        Type = (int)request.Type,
        IsActive = true,
        CreatedAtUtc = DateTime.UtcNow,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        EmailTemplateTexts = request.EmailTemplateTexts
          .Select(x => _dbEmailTemplateTextMapper.Map(x, templateId))
          .ToList()
      };
    }
  }
}
