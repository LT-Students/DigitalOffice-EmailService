using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Business.Commands.ModuleSetting.Interfaces;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.ModuleSetting;
using LT.DigitalOffice.EmailService.Validation.Validators.ModuleSetting.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EmailService.Business.Commands.ModuleSetting
{
  public class EditModuleSettingCommand : IEditModuleSettingCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IResponseCreator _responseCreator;
    private readonly IEditModuleSettingRequestValidator _validator;
    private readonly ISmtpSettingsRepository _repository;
    private readonly IPatchDbModuleSettingMapper _mapper;

    public EditModuleSettingCommand(
      IAccessValidator accessValidator,
      IResponseCreator responseCreator,
      IEditModuleSettingRequestValidator validator,
      ISmtpSettingsRepository repository,
      IPatchDbModuleSettingMapper mapper)
    {
      _accessValidator = accessValidator;
      _responseCreator = responseCreator;
      _validator = validator;
      _repository = repository;
      _mapper = mapper;
    }
    public async Task<OperationResultResponse<bool>> ExecuteAsync(
      Guid moduleSettingId,
      JsonPatchDocument<EditModuleSettingRequest> patch)
    {
      if (!await _accessValidator.IsAdminAsync())
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      if (!_validator.ValidateCustom(patch, out List<string> errors))
      {
        return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest, errors);
      }

      OperationResultResponse<bool> response = new();

      response.Body = await _repository.EditAsync(moduleSettingId, _mapper.Map(patch));
      response.Status = OperationResultStatusType.FullSuccess;

      if (!response.Body)
      {
        response = _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest);
      }

      return response;
    }
  }
}
