using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Business.Commands.EmailTemplate.Interfaces;
using LT.DigitalOffice.EmailService.Models.Dto.Models;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.EmailService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class EmailTemplateController : ControllerBase
  {
    [HttpPost("create")]
    public async Task<OperationResultResponse<Guid?>> Create(
      [FromServices] ICreateEmailTemplateCommand command,
      [FromBody] EmailTemplateRequest request)
    {
      return await command.Execute(request);
    }

    [HttpPatch("edit")]
    public async Task<OperationResultResponse<bool>> Edit(
      [FromServices] IEditEmailTemplateCommand command,
      [FromQuery] Guid emailTemplateId,
      [FromBody] JsonPatchDocument<EditEmailTemplateRequest> patch)
    {
      return await command.Execute(emailTemplateId, patch);
    }

    [HttpGet("find")]
    public async Task<FindResultResponse<EmailTemplateInfo>> Find(
      [FromServices] IFindEmailTemplateCommand command,
      [FromQuery] FindEmailTemplateFilter filter)
    {
      return await command.Execute(filter);
    }
  }
}

