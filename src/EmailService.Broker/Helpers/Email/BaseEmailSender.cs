using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.EmailService.Models.Dto.Helpers;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.EmailService.Broker.Helpers
{
  public abstract class BaseEmailSender
  {
    private readonly ISmtpSettingsRepository _repository;
    protected readonly ILogger _logger;

    private async Task<bool> GetSmtpCredentialsAsync()
    {
      DbModuleSetting result = await _repository.GetAsync();

      if (result != null)
      {
        SmtpCredentials.Host = result.Host;
        SmtpCredentials.Port = result.Port;
        SmtpCredentials.Email = result.Email;
        SmtpCredentials.Password = result.Password;
        SmtpCredentials.EnableSsl = result.EnableSsl;

        return true;
      }
 
      _logger?.LogError("Cannot get smtp credentials.");

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
      ISmtpSettingsRepository repository,
      ILogger logger)
    {
      _repository = repository;
      _logger = logger;
    }
  }
}
