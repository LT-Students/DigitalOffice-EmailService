using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.EmailService.Broker.Helpers
{
  public class EmailSender : BaseEmailSender
  {
    private readonly IEmailRepository _emailRepository;
    private readonly IUnsentEmailRepository _unsentEmailRepository;

    public EmailSender(
      ILogger<EmailSender> logger,
      ISmtpSettingsRepository getSmtpCredentials,
      IEmailRepository emailRepository,
      IUnsentEmailRepository unsentEmailRepository)
    : base(getSmtpCredentials, logger)
    {
      _emailRepository = emailRepository;
      _unsentEmailRepository = unsentEmailRepository;
    }

    public async Task<bool> SendEmailAsync(
      string receiver,
      string subject,
      string text,
      Guid? senderId = null)
    {
      DbEmail dbEmail = new()
      {
        Id = Guid.NewGuid(),
        SenderId = senderId,
        Receiver = receiver,
        Subject = subject,
        Text = text,
        CreatedAtUtc = DateTime.UtcNow
      };

      await _emailRepository.SaveEmailAsync(dbEmail);

      if (await SendAsync(dbEmail))
      {
        return true;
      }

      await _unsentEmailRepository.CreateAsync(
        new DbUnsentEmail
        {
          Id = Guid.NewGuid(),
          CreatedAtUtc = dbEmail.CreatedAtUtc,
          LastSendAtUtc = dbEmail.CreatedAtUtc,
          EmailId = dbEmail.Id,
          TotalSendingCount = 1
        });

      return false;
    }

    public async Task<bool> ResendEmail(Guid unsentEmailId)
    {
      DbUnsentEmail dbUnsentEmail = await _unsentEmailRepository.GetAsync(unsentEmailId);

      if (await SendAsync(dbUnsentEmail.Email))
      {
        await _unsentEmailRepository.RemoveAsync(dbUnsentEmail);

        return true;
      }

      await _unsentEmailRepository.IncrementTotalCountAsync(dbUnsentEmail);

      return false;
    }
  }
}
