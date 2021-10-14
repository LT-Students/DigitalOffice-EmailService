using FluentValidation;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.EmailService.Validation.Validators.EmailTemplate.Interfaces;

namespace LT.DigitalOffice.EmailService.Validation.Validators.EmailTemplate
{
  public class CreateEmailTemplateValidator : AbstractValidator<EmailTemplateRequest>, ICreateEmailTemplateValidator
  {
    public CreateEmailTemplateValidator()
    {
      RuleFor(et => et.Name)
        .NotEmpty().WithMessage("Email template name must not be empty.");

      RuleFor(et => et.Type)
        .IsInEnum().WithMessage("Incorrect Email template type.");

      RuleFor(et => et.EmailTemplateTexts)
        .NotEmpty().WithMessage("Email template texts must not be empty.");

      RuleForEach(et => et.EmailTemplateTexts)
        .Must(ett => ett != null).WithMessage("Email template text must not be null.")
        .ChildRules(ett =>
        {
          ett.RuleFor(ett => ett.Subject)
            .NotEmpty().WithMessage("Subject must not be empty.");

          ett.RuleFor(ett => ett.Text)
            .NotEmpty().WithMessage("Text must not be empty.");

          ett.RuleFor(ett => ett.Language)
            .NotEmpty().WithMessage("Language must not be empty.")
            .Must(ett => ett.Trim().Length != 2).WithMessage("Language must contain two letters.");
        });
    }
  }
}
