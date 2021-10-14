using FluentValidation;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.EmailService.Validation.Validators.EmailTemplateText.Interfaces;

namespace LT.DigitalOffice.EmailService.Validation.Validators.EmailTemplateText
{
  public class CreateEmailTemplateTextValidator : AbstractValidator<EmailTemplateTextRequest>, ICreateEmailTemplateTextValidator
  {
    public CreateEmailTemplateTextValidator()
    {
      RuleFor(ett => ett.EmailTemplateId)
        .NotEmpty().WithMessage("Email template id must not be empty.");

      RuleFor(ett => ett.Subject)
        .NotEmpty().WithMessage("Subject must not be empty.");

      RuleFor(ett => ett.Text)
        .NotEmpty().WithMessage("Text must not be empty.");

      RuleFor(ett => ett.Language)
        .NotEmpty().WithMessage("Language must not be empty.")
        .Must(ett => ett.Trim().Length != 2).WithMessage("Language must contain two letters.");
    }
  }
}
