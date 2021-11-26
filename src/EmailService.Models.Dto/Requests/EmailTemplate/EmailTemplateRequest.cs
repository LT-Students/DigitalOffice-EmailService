using System.Collections.Generic;
using LT.DigitalOffice.Models.Broker.Enums;

namespace LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate
{
  public record EmailTemplateRequest
  {
    public string Name { get; set; }
    public EmailTemplateType Type { get; set; }
    public List<EmailTemplateTextRequest> EmailTemplateTexts { get; set; }
  }
}
