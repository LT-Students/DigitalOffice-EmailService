using FluentValidation;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.EmailService.Validation.Validators.EmailTemplate.Interfaces;

namespace LT.DigitalOffice.EmailService.Validation.Validators.EmailTemplate
{
  public class CreateEmailTemplateValidator : AbstractValidator<EmailTemplateRequest>, ICreateEmailTemplateValidator
  {
    public CreateEmailTemplateValidator()
    {
      RuleFor(et => et.Name.Trim())
        .NotEmpty().WithMessage("Email template name must not be empty.");

      RuleFor(et => et.Type)
        .IsInEnum().WithMessage("Incorrect Email template type.");

      RuleFor(et => et.EmailTemplateTexts)
        .NotNull().WithMessage("Email template texts must not be null.");

      RuleForEach(et => et.EmailTemplateTexts)
        .Must(ett => ett != null).WithMessage("Email template text must not be null.")
        .ChildRules(ett =>
        {
          ett.RuleFor(ett => ett.Subject.Trim())
            .NotEmpty().WithMessage("Subject must not be empty.");

          ett.RuleFor(ett => ett.Text.Trim())
            .NotEmpty().WithMessage("Text must not be empty.");

          ett.RuleFor(ett => ett.Language.Trim())
            .NotEmpty().WithMessage("Language must not be empty.")
            .MaximumLength(2).WithMessage("Language is to long.");
        });
    }
  }
}
