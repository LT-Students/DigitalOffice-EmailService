using System;

namespace LT.DigitalOffice.EmailService.Models.Dto.Models
{
  public record EmailTemplateTextInfo
  {
    public Guid Id { get; set; }
    public string Subject { get; set; }
    public string Text { get; set; }
    public string Language { get; set; }
  }
}
