using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Business.Commands.UnsentEmail.Interfaces;
using LT.DigitalOffice.EmailService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Requests;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.EmailService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class UnsentEmailController : ControllerBase
  {
    [HttpDelete("resend")]
    public async Task<OperationResultResponse<bool>> ResendAsync(
      [FromServices] IResendEmailCommand command,
      [FromQuery] Guid unsentEmailId)
    {
      return await command.ExecuteAsync(unsentEmailId);
    }

    [HttpGet("find")]
    public async Task<FindResultResponse<UnsentEmailInfo>> FindAsync(
      [FromServices] IFindUnsentEmailsCommand command,
      [FromQuery] BaseFindFilter filter)
    {
      return await command.ExecuteAsync(filter);
    }
  }
}

