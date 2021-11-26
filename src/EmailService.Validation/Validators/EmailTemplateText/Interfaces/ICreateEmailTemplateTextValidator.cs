using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;

namespace LT.DigitalOffice.EmailService.Validation.Validators.EmailTemplateText.Interfaces
{
  [AutoInject]
  public interface ICreateEmailTemplateTextValidator : IValidator<EmailTemplateTextRequest>
  {
  }
}
