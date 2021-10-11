using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplateText;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EmailService.Validation.Validators.EmailTemplateText.Interfaces
{
  [AutoInject]
  public interface IEditEmailTemplateTextValidator : IValidator<JsonPatchDocument<EditEmailTemplateTextRequest>>
  {
  }
}
