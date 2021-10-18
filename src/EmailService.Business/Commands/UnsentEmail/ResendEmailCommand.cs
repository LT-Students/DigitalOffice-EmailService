using System;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Broker.Helpers;
using LT.DigitalOffice.EmailService.Business.Commands.UnsentEmail.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EmailService.Business.Commands.UnsentEmail
{
  public class ResendEmailCommand : IResendEmailCommand
  {
    private readonly IAccessValidator _accessValidator;
    private readonly EmailSender _emailSender;
    private readonly IResponseCreater _responseCreater;

    public ResendEmailCommand(
      IAccessValidator accessValidator,
      EmailSender emailSender,
      IResponseCreater responseCreater)
    {
      _accessValidator = accessValidator;
      _emailSender = emailSender;
      _responseCreater = responseCreater;
    }

    public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid id)
    {
      if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveEmailTemplates))
      {
        return _responseCreater.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
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
