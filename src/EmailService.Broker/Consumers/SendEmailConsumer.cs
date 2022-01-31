using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Broker.Helpers;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Email;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.EmailService.Broker.Consumers
{
  public class SendEmailConsumer : IConsumer<ISendEmailRequest>
  {
    private readonly ILogger<SendEmailConsumer> _logger;
    private readonly EmailSender _sender;

    private async Task<bool> SendEmailAsync(ISendEmailRequest request)
    {
      _logger.LogInformation(
        "Start email sending to '{Receiver}'.",
        request.Receiver);

      return request is null
        ? false
        : await _sender.SendEmailAsync(request.Receiver, request.Subject, request.Text);
    }

    public SendEmailConsumer(
      ILogger<SendEmailConsumer> logger,
      EmailSender sender)
    {
      _logger = logger;
      _sender = sender;
    }

    public async Task Consume(ConsumeContext<ISendEmailRequest> context)
    {
      Object response = OperationResultWrapper.CreateResponse(SendEmailAsync, context.Message);

      await context.RespondAsync<IOperationResult<bool>>(response);
    }
  }
}
