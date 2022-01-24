using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Business.Commands.ModuleSetting.Interfaces;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.ModuleSetting;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.EmailService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class ModuleSettingController : ControllerBase
  {
    [HttpPatch("edit")]
    public async Task<OperationResultResponse<bool>> EditAsync(
      [FromServices] IEditModuleSettingCommand command,
      [FromQuery] Guid moduleSettingId,
      [FromBody] JsonPatchDocument<EditModuleSettingRequest> patch)
    {
      return await command.ExecuteAsync(moduleSettingId, patch);
    }
  }
}
