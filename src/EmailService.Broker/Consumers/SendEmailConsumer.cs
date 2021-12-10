using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Broker.Helpers;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.EmailService.Broker.Consumers
{
  public class SendEmailConsumer : IConsumer<ISendEmailRequest>
  {
    private readonly ILogger<SendEmailConsumer> _logger;
    private readonly IEmailTemplateRepository _templateRepository;
    private readonly EmailSender _sender;

    private async Task<bool> SendEmailAsync(ISendEmailRequest request)
    {
      _logger.LogInformation(
        "Start email sending to '{receiver}'.",
        request.Email);

      DbEmailTemplateText dbEmailTemplateText = await GetDbEmailTemplateTextAsync(request);

      if (dbEmailTemplateText == null)
      {
        return false;
      }

      string subject = GetParsedEmailTemplateText(request.TemplateValues, dbEmailTemplateText.Subject);
      string body = GetParsedEmailTemplateText(request.TemplateValues, dbEmailTemplateText.Text);

      return await _sender.SendEmailAsync(request.Email, subject, body);
    }

    private async Task<DbEmailTemplateText> GetDbEmailTemplateTextAsync(ISendEmailRequest request)
    {
      DbEmailTemplate dbEmailTemplate = null;
      if (request.TemplateId != null)
      {
        dbEmailTemplate = await _templateRepository.GetAsync(request.TemplateId.Value);
      }
      else
      {
        dbEmailTemplate = await _templateRepository.GetAsync((int)request.Type);
      }

      DbEmailTemplateText dbEmailTemplateText = dbEmailTemplate?.EmailTemplateTexts
        .FirstOrDefault(ett => ett.Language == request.Language);

      if (dbEmailTemplateText == null)
      {
        _logger.LogWarning("Email template text was not found.");

        return null;
      }

      return dbEmailTemplateText;
    }

    private string GetParsedEmailTemplateText(IDictionary<string, string> values, string text)
    {
      string[] strArray = text.Split('{', '}');

      for (int i = 0; i < strArray.Length; i++)
      {
        if (values.TryGetValue(strArray[i], out string value))
        {
          strArray[i] = value;
        }
      }

      StringBuilder sb = new();
      sb.AppendJoin("", strArray);

      return sb.ToString();
    }

    public SendEmailConsumer(
      ILogger<SendEmailConsumer> logger,
      IEmailTemplateRepository templateRepository,
      EmailSender sender)
    {
      _logger = logger;
      _templateRepository = templateRepository;
      _sender = sender;
    }

    public async Task Consume(ConsumeContext<ISendEmailRequest> context)
    {
      Object response = OperationResultWrapper.CreateResponse(SendEmailAsync, context.Message);

      await context.RespondAsync<IOperationResult<bool>>(response);
    }
  }
}
