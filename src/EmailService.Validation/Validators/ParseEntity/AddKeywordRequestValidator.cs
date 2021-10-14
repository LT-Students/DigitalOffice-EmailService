using FluentValidation;
using LT.DigitalOffice.EmailService.Broker.Helpers.ParseEntity;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.ParseEntity;
using LT.DigitalOffice.EmailService.Validation.Validators.ParseEntity.Interfaces;

namespace LT.DigitalOffice.EmailService.Validation.Validators.ParseEntity
{
  public class AddKeywordRequestValidator : AbstractValidator<CreateKeywordRequest>, IAddKeywordRequestValidator
  {
    public AddKeywordRequestValidator(IKeywordRepository repository)
    {
      RuleFor(x => x.Keyword)
        .NotEmpty().WithMessage("Keyword must not be empty.")
        .MaximumLength(50).WithMessage("Keyword is to long.")
        .MustAsync(async (k, cancellation) => !(await repository.DoesKeywordExistAsync(k)))
        .WithMessage("The keyword already exists.");

      RuleFor(x => x.ServiceName)
        .IsInEnum().WithMessage("Incorrect service name type.");

      RuleFor(x => x.EntityName)
        .NotEmpty().WithMessage("Entity name must not be empty.");

      RuleFor(x => x.PropertyName)
        .NotEmpty().WithMessage("Property name must not be empty.");

      RuleFor(x => x)
        .Must(x => AllParseEntities.Entities.ContainsKey(x.ServiceName.ToString())
          && AllParseEntities.Entities[x.ServiceName.ToString()].ContainsKey("Db" + x.EntityName)
          && AllParseEntities.Entities[x.ServiceName.ToString()]["Db" + x.EntityName].Contains(x.PropertyName))
        .WithMessage("No entity with requested property in the service.");
    }
  }
}
