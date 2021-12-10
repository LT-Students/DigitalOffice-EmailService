using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Helpers;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.EmailService.Broker.Helpers
{
  public abstract class BaseEmailSender
  {
    private readonly IRequestClient<IGetSmtpCredentialsRequest> _rcGetSmtpCredentials;
    protected readonly ILogger _logger;

    private async Task<bool> GetSmtpCredentialsAsync()
    {
      string logMessage = "Cannot get smtp credentials.";

      try
      {
        IOperationResult<IGetSmtpCredentialsResponse> result =
          (await _rcGetSmtpCredentials.GetResponse<IOperationResult<IGetSmtpCredentialsResponse>>(
            IGetSmtpCredentialsRequest.CreateObj())).Message;

        if (result.IsSuccess)
        {
          SmtpCredentials.Host = result.Body.Host;
          SmtpCredentials.Port = result.Body.Port;
          SmtpCredentials.Email = result.Body.Email;
          SmtpCredentials.Password = result.Body.Password;
          SmtpCredentials.EnableSsl = result.Body.EnableSsl;

          return true;
        }

        _logger?.LogWarning(logMessage);
      }
      catch (Exception exc)
      {
        _logger?.LogError(exc, logMessage);
      }

      return false;
    }

    protected async Task<bool> SendAsync(DbEmail dbEmail)
    {
      if (!SmtpCredentials.HasValue && !(await GetSmtpCredentialsAsync()))
      {
        return false;
      }

      try
      {
        var message = new MailMessage(
        SmtpCredentials.Email,
        dbEmail.Receiver)
        {
          Subject = dbEmail.Subject,
          Body = dbEmail.Body
        };

        SmtpClient smtp = new SmtpClient(
          SmtpCredentials.Host,
          SmtpCredentials.Port)
        {
          Credentials = new NetworkCredential(
            SmtpCredentials.Email,
            SmtpCredentials.Password),
          EnableSsl = SmtpCredentials.EnableSsl
        };

        smtp.Send(message);
      }
      catch (Exception exc)
      {
        _logger?.LogError(exc,
          "Errors while sending email with id {emailId} to {to}. Email replaced to resend queue.",
          dbEmail.Id,
          dbEmail.Receiver);

        return false;
      }

      return true;
    }

    public BaseEmailSender(
      IRequestClient<IGetSmtpCredentialsRequest> rcGetSmtpCredentials,
      ILogger logger)
    {
      _rcGetSmtpCredentials = rcGetSmtpCredentials;
      _logger = logger;
    }
  }
}
