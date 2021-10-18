using System;
using LT.DigitalOffice.EmailService.Models.Dto.Enums;

namespace LT.DigitalOffice.EmailService.Models.Dto
{
  public record KeywordInfo
  {
    public Guid Id { get; set; }
    public string Keyword { get; set; }
    public ServiceName ServiceName { get; set; }
    public string EntityName { get; set; }
    public string PropertyName { get; set; }
  }
}
