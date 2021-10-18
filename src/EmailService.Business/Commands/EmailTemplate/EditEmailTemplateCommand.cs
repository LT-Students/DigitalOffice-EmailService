using System;
using System.Collections.Generic;
using System.Net;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.EmailService.Business.Commands.EmailTemplate.Interfaces;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.EmailService.Validation.Validators.EmailTemplate.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;

namespace LT.DigitalOffice.EmailService.Business.Commands.EmailTemplate
{
  public class EditEmailTemplateCommand : IEditEmailTemplateCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IEmailTemplateRepository _repository;
    private readonly IEditEmailTemplateValidator _validator;
    private readonly IPatchDbEmailTemplateMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreater _responseCreater;

    public EditEmailTemplateCommand(
      IAccessValidator accessValidator,
      IEmailTemplateRepository repository,
      IEditEmailTemplateValidator validator,
      IPatchDbEmailTemplateMapper mapper,
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
      Guid emailTemplateId,
      JsonPatchDocument<EditEmailTemplateRequest> patch)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveEmailTemplates))
      {
        return _responseCreater.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
      }

      if (!_validator.ValidateCustom(patch, out List<string> errors))
      {
        return _responseCreater.CreateFailureResponse<bool>(HttpStatusCode.BadRequest, errors);
      }

      OperationResultResponse<bool> response = new();

      response.Body = await _repository.EditAsync(emailTemplateId, _mapper.Map(patch));

      response.Status = OperationResultStatusType.FullSuccess;

      if (!response.Body)
      {
        response = _responseCreater.CreateFailureResponse<bool>(HttpStatusCode.BadRequest);
      }

      return response;
    }
  }
}
