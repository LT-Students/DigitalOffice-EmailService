using FluentValidation;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EmailService.Validation.Validators.EmailTemplate.Interfaces
{
  [AutoInject]
  public interface IEditEmailTemplateValidator : IValidator<JsonPatchDocument<EditEmailTemplateRequest>>
  {
  }
}
