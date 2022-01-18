using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.EmailService.Broker.Helpers
{
  public class EmailResender : BaseEmailSender
  {
    private readonly IUnsentEmailRepository _unsentEmailRepository;

    public async Task StartResend(int intervalInMinutes, int maxResendingCount)
    {
      while (true)
      {
        List<DbUnsentEmail> unsentEmails = await _unsentEmailRepository.GetAllAsync(maxResendingCount);

        foreach (var email in unsentEmails)
        {
          if (await SendAsync(email.Email))
          {
            await _unsentEmailRepository.RemoveAsync(email);
          }
          else
          {
            await _unsentEmailRepository.IncrementTotalCountAsync(email);
          }
        }

        await Task.Delay(TimeSpan.FromMinutes(intervalInMinutes));
      }
    }

    public EmailResender(
      IUnsentEmailRepository unsentEmailRepository,
      ILogger<EmailResender> logger,
      ISmtpSettingsRepository rcGetSmtpCredentials)
    : base(rcGetSmtpCredentials, logger)
    {
      _unsentEmailRepository = unsentEmailRepository;
    }
  }
}
