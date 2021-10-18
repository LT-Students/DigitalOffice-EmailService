using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Business.Commands.EmailTemplate.Interfaces;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.EmailService.Validation.Validators.EmailTemplate.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EmailService.Business.Commands.EmailTemplate
{
  public class CreateEmailTemplateCommand : ICreateEmailTemplateCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly ICreateEmailTemplateValidator _validator;
    private readonly IDbEmailTemplateMapper _mapper;
    private readonly IEmailTemplateRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IResponseCreater _responseCreater;

    public CreateEmailTemplateCommand(
      IAccessValidator accessValidator,
      ICreateEmailTemplateValidator validator,
      IDbEmailTemplateMapper mapper,
      IEmailTemplateRepository repository,
      IHttpContextAccessor httpContextAccessor,
      IResponseCreater responseCreater)
    {
      _accessValidator = accessValidator;
      _validator = validator;
      _mapper = mapper;
      _repository = repository;
      _httpContextAccessor = httpContextAccessor;
      _responseCreater = responseCreater;
    }

    public async Task<OperationResultResponse<Guid?>> ExecuteAsync(EmailTemplateRequest request)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveEmailTemplates))
      {
        return _responseCreater.CreateFailureResponse<Guid?>(HttpStatusCode.Forbidden);
      }

      if (!_validator.ValidateCustom(request, out List<string> errors))
      {
        return _responseCreater.CreateFailureResponse<Guid?>(HttpStatusCode.BadRequest, errors);
      }

      OperationResultResponse<Guid?> response = new();

      response.Body = await _repository.CreateAsync(_mapper.Map(request));
      response.Status = OperationResultStatusType.FullSuccess;

      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      if (response.Body == null)
      {
        response = _responseCreater.CreateFailureResponse<Guid?>(HttpStatusCode.BadRequest);
      }

      return response;
    }
  }
}
