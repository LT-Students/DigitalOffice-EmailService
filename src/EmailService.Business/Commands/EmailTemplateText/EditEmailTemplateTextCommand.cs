using System;
using System.Collections.Generic;
using System.Net;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.EmailService.Business.Commands.EmailTemplateText.Interfaces;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplateText;
using LT.DigitalOffice.EmailService.Validation.Validators.EmailTemplateText.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;

namespace LT.DigitalOffice.EmailService.Business.Commands.EmailTemplateText
{
  public class EditEmailTemplateTextCommand : IEditEmailTemplateTextCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IEmailTemplateTextRepository _repository;
    private readonly IEditEmailTemplateTextValidator _validator;
    private readonly IPatchDbEmailTemplateTextMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreater _responseCreater;

    public EditEmailTemplateTextCommand(
      IAccessValidator accessValidator,
      IEmailTemplateTextRepository repository,
      IEditEmailTemplateTextValidator validator,
      IPatchDbEmailTemplateTextMapper mapper,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreater responseCreater)
    {
      _validator = validator;
      _repository = repository;
      _accessValidator = accessValidator;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
      _responseCreater = responseCreater;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(
      Guid emailTemplateTextId,
      JsonPatchDocument<EditEmailTemplateTextRequest> patch)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveEmailTemplates))
      {
        return _responseCreater.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      if (!_validator.ValidateCustom(patch, out List<string> errors))
      {
        return _responseCreater.CreateFailureResponse<bool>(
          HttpStatusCode.BadRequest,
          errors);
      }

      OperationResultResponse<bool> response = new();

      response.Body = await _repository.EditAsync(emailTemplateTextId, _mapper.Map(patch));
      response.Status = OperationResultStatusType.FullSuccess;

      if (!response.Body)
      {
        response = _responseCreater.CreateFailureResponse<bool>(HttpStatusCode.BadRequest);
      }

      return response;
    }
  }
}
