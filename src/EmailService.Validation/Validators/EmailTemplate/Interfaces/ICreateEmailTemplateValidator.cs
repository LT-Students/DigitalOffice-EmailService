using FluentValidation;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EmailService.Validation.Validators.EmailTemplate.Interfaces
{
  [AutoInject]
  public interface ICreateEmailTemplateValidator : IValidator<EmailTemplateRequest>
  {
  }
}
