using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.ParseEntity;

namespace LT.DigitalOffice.EmailService.Validation.Validators.ParseEntity.Interfaces
{
  [AutoInject]
  public interface IAddKeywordRequestValidator : IValidator<AddKeywordRequest>
  {
  }
}
