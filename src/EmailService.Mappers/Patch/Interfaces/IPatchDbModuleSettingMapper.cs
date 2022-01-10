using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.ModuleSetting;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EmailService.Mappers.Patch.Interfaces
{
  [AutoInject]
  public interface IPatchDbModuleSettingMapper
  {
    JsonPatchDocument<DbModuleSetting> Map(
      JsonPatchDocument<EditModuleSettingRequest> request);
  }
}
