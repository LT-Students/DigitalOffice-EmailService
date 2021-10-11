using FluentValidation;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.EmailService.Validation.Validators.EmailTemplateText.Interfaces;

namespace LT.DigitalOffice.EmailService.Validation.Validators.EmailTemplateText
{
  public class CreateEmailTemplateTextValidator : AbstractValidator<EmailTemplateTextRequest>, ICreateEmailTemplateTextValidator
  {
    public CreateEmailTemplateTextValidator()
    {
      RuleFor(x => x.EmailTemplateId)
        .NotEmpty().WithMessage("Email template id must not be empty.");

      RuleFor(x => x.Subject.Trim())
        .NotEmpty().WithMessage("Subject must not be empty.");

      RuleFor(x => x.Text.Trim())
        .NotEmpty().WithMessage("Text must not be empty.");

      RuleFor(x => x.Language.Trim())
        .NotEmpty().WithMessage("Language must not be empty.")
        .MaximumLength(2).WithMessage("Language is to long.");
    }
  }
}
