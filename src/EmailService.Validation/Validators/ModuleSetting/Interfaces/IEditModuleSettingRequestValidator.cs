using FluentValidation;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.ModuleSetting;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EmailService.Validation.Validators.ModuleSetting.Interfaces
{
  [AutoInject]
  public interface IEditModuleSettingRequestValidator : IValidator<JsonPatchDocument<EditModuleSettingRequest>>
  {
  }
}
