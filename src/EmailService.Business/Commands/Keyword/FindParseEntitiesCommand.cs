using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Broker.Helpers.ParseEntity;
using LT.DigitalOffice.EmailService.Business.Commands.ParseEntity.Interface;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EmailService.Business.Commands.ParseEntity
{
  public class FindParseEntitiesCommand : IFindParseEntitiesCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreater _responseCreater;

    public FindParseEntitiesCommand(
      IAccessValidator accessValidator,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreater responseCreater)
    {
      _accessValidator = accessValidator;
      _httpContextAccessor = httpContextAccessor;
      _responseCreater = responseCreater;
    }

    public async Task<OperationResultResponse<Dictionary<string, Dictionary<string, List<string>>>>> ExecuteAsync()
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveEmailTemplates))
      {
        return _responseCreater
          .CreateFailureResponse<Dictionary<string, Dictionary<string, List<string>>>>(
            HttpStatusCode.Forbidden);
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

      return new()
      {
        Status = OperationResultStatusType.FullSuccess,
        Body = response
      };
    }
  }
}
