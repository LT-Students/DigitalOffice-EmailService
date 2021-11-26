using LT.DigitalOffice.Kernel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate
{
  public record FindEmailTemplateFilter : BaseFindFilter
  {
    [FromQuery(Name = "includedeactivated")]
    public bool IncludeDeactivated { get; set; } = false;
  }
}
