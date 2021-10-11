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

namespace LT.DigitalOffice.EmailService.Business.Commands.EmailTemplateText
{
  public class EditEmailTemplateTextCommand : IEditEmailTemplateTextCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly IEmailTemplateTextRepository _repository;
    private readonly IEditEmailTemplateTextValidator _validator;
    private readonly IPatchDbEmailTemplateTextMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EditEmailTemplateTextCommand(
      IAccessValidator accessValidator,
      IEmailTemplateTextRepository repository,
      IEditEmailTemplateTextValidator validator,
      IPatchDbEmailTemplateTextMapper mapper,
      IHttpContextAccessor httpContextAccessor)
    {
      _validator = validator;
      _repository = repository;
      _accessValidator = accessValidator;
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OperationResultResponse<bool>> Execute(
      Guid emailTemplateTextId,
      JsonPatchDocument<EditEmailTemplateTextRequest> patch)
    {
      if (!_accessValidator.HasRights(Rights.AddEditRemoveEmailTemplates))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        return new OperationResultResponse<bool>
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Not enough rights." }
        };
      }

      if (!_validator.ValidateCustom(patch, out List<string> errors))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new OperationResultResponse<bool>
        {
          Status = OperationResultStatusType.Failed,
          Errors = errors
        };
      }

      OperationResultResponse<bool> response = new();

      response.Body = await _repository.EditAsync(emailTemplateTextId, _mapper.Map(patch));
      response.Status = OperationResultStatusType.FullSuccess;

      if (!response.Body)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Status = OperationResultStatusType.Failed;
      }

      return response;
    }
  }
}
