using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Business.Commands.ParseEntity.Interface;
using LT.DigitalOffice.EmailService.Models.Dto;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.ParseEntity;
using LT.DigitalOffice.Kernel.Requests;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.EmailService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class KeywordController : ControllerBase
  {

    [HttpGet("FindParseEntities")]
    public async Task<OperationResultResponse<Dictionary<string, Dictionary<string, List<string>>>>> FindParseEntities(
      [FromServices] IFindParseEntitiesCommand command)
    {
      return await command.Execute();
    }

    [HttpGet("Find")]
    public async Task<FindResultResponse<KeywordInfo>> FindParsedProperties(
      [FromServices] IFindKeywordCommand command,
      [FromQuery] BaseFindFilter filter)
    {
      return await command.Execute(filter);
    }

    [HttpPost("Add")]
    public async Task<OperationResultResponse<Guid?>> AddKeyword(
      [FromServices] IAddKeywordCommand command,
      [FromBody] AddKeywordRequest request)
    {
      return await command.Execute(request);
    }
  }
}

