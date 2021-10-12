using System;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Broker.Helpers;
using LT.DigitalOffice.EmailService.Business.Commands.UnsentEmail.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EmailService.Business.Commands.UnsentEmail
{
  public class ResendEmailCommand : IResendEmailCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly EmailSender _emailSender;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ResendEmailCommand(
      IAccessValidator accessValidator,
      EmailSender emailSender,
      IHttpContextAccessor httpContextAccessor)
    {
      _accessValidator = accessValidator;
      _emailSender = emailSender;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid id)
    {
      if (!(await _accessValidator.HasRightsAsync(Rights.AddEditRemoveEmailTemplates)))
      {
        _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        return new()
        {
          Status = OperationResultStatusType.Failed,
          Errors = new() { "Not enough rights." }
        };
      }

      bool isSuccess = await _emailSender.ResendEmail(id);

      return new()
      {
        Status = isSuccess ? OperationResultStatusType.FullSuccess : OperationResultStatusType.Failed,
        Body = isSuccess
      };
    }
  }
}
