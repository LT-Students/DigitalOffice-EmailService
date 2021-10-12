using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Broker.Helpers.ParseEntity;
using LT.DigitalOffice.EmailService.Business.Commands.ParseEntity.Interface;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EmailService.Business.Commands.ParseEntity
{
  public class FindParseEntitiesCommand : IFindParseEntitiesCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FindParseEntitiesCommand(
      IAccessValidator accessValidator,
      IHttpContextAccessor httpContextAccessor)
    {
      _accessValidator = accessValidator;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OperationResultResponse<Dictionary<string, Dictionary<string, List<string>>>>> ExecuteAsync()
    {
      if (!(await _accessValidator.HasRightsAsync(Rights.AddEditRemoveEmailTemplates)))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        return new OperationResultResponse<Dictionary<string, Dictionary<string, List<string>>>>
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Not enough rights." }
        };
      }

      Dictionary<string, Dictionary<string, List<string>>> response = new();

      foreach (KeyValuePair<string, Dictionary<string, List<string>>> service in AllParseEntities.Entities)
      {
        response.Add(service.Key, new());

        foreach (KeyValuePair<string, List<string>> entity in service.Value)
        {
          response[service.Key].Add(entity.Key[2..], entity.Value);
        }
      }

      return new OperationResultResponse<Dictionary<string, Dictionary<string, List<string>>>>
      {
        Status = OperationResultStatusType.FullSuccess,
        Body = response
      };
    }
  }
}
