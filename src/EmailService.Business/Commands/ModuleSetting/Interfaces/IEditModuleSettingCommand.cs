using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.ModuleSetting;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EmailService.Business.Commands.ModuleSetting.Interfaces
{
  [AutoInject]
  public interface IEditModuleSettingCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(
      Guid moduleSettingId,
      JsonPatchDocument<EditModuleSettingRequest> patch);
  }
}
