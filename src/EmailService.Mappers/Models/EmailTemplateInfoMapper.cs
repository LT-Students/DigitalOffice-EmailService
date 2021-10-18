using System.Linq;
using LT.DigitalOffice.EmailService.Mappers.Models.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Enums;

namespace LT.DigitalOffice.EmailService.Mappers.Models
{
  public class EmailTemplateInfoMapper : IEmailTemplateInfoMapper
  {
    private readonly IEmailTemplateTextInfoMapper _mapper;

    public EmailTemplateInfoMapper(
      IEmailTemplateTextInfoMapper mapper)
    {
      _mapper = mapper;
    }

    public EmailTemplateInfo Map(DbEmailTemplate dbEmailTemplate)
    {
      if (dbEmailTemplate == null)
      {
        return null;
      }

      return new EmailTemplateInfo
      {
        Id = dbEmailTemplate.Id,
        Name = dbEmailTemplate.Name,
        Type = (EmailTemplateType)dbEmailTemplate.Type,
        IsActive = dbEmailTemplate.IsActive,
        CreatedBy = dbEmailTemplate.CreatedBy,
        CreatedAtUtc = dbEmailTemplate.CreatedAtUtc,
        Texts = dbEmailTemplate.EmailTemplateTexts?
          .Select(_mapper.Map)
          .ToList()
      };
    }
  }
}
