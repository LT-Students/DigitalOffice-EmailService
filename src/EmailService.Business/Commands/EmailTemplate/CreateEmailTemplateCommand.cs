﻿using System;
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

    public CreateEmailTemplateCommand(
      IAccessValidator accessValidator,
      ICreateEmailTemplateValidator validator,
      IDbEmailTemplateMapper mapper,
      IEmailTemplateRepository repository,
      IHttpContextAccessor httpContextAccessor)
    {
      _accessValidator = accessValidator;
      _validator = validator;
      _mapper = mapper;
      _repository = repository;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OperationResultResponse<Guid?>> ExecuteAsync(EmailTemplateRequest request)
    {
      if (!(await _accessValidator.HasRightsAsync(Rights.AddEditRemoveEmailTemplates)))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        return new()
        {
          Status = OperationResultStatusType.Failed
        };
      }

      if (!_validator.ValidateCustom(request, out List<string> errors))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return new()
        {
          Status = OperationResultStatusType.Failed,
          Errors = errors
        };
      }

      OperationResultResponse<Guid?> response = new();

      response.Body = await _repository.CreateAsync(_mapper.Map(request));

      if (response.Body == null)
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        response.Status = OperationResultStatusType.Failed;
        return response;
      }

      _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

      response.Status = OperationResultStatusType.FullSuccess;
      return response;
    }
  }
}
