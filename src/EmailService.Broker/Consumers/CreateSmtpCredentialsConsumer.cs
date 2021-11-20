using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Data.Interfaces;
using LT.DigitalOffice.EmailService.Models.Db;
using LT.DigitalOffice.Kernel.Broker;
using MassTransit;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using Microsoft.Extensions.Caching.Memory;

namespace LT.DigitalOffice.EmailService.Broker.Consumers
{
  public class CreateSmtpCredentialsConsumer : IConsumer<ICreateSmtpCredentialsRequest>
  {
    private readonly IMemoryCache _cache;
    private readonly ISmtpSettingsRepository _repository;
    private async Task<object> CreateCredentials(ICreateSmtpCredentialsRequest request)
    {
      DbModuleSetting dbModuleSetting = new DbModuleSetting()
      {
        Id = Guid.NewGuid(),
        Host = request.Host,
        Port = request.Port,
        EnableSsl = request.EnableSsl,
        Email = request.Email,
        Password = request.Password,
        CreatedBy = request.CreatedBy,
        CreatedAtUtc = DateTime.UtcNow,
      };

      _cache.Set(dbModuleSetting.Id, dbModuleSetting);
      await _repository.CreateAsync(dbModuleSetting);

      return true;
    }

    public CreateSmtpCredentialsConsumer(
      IMemoryCache cache,
      ISmtpSettingsRepository repository)
    {
      _cache = cache;
      _repository = repository;
    }
    public async Task Consume(ConsumeContext<ICreateSmtpCredentialsRequest> context)
    {
      Object result = OperationResultWrapper.CreateResponse(CreateCredentials, context.Message);

      await context.RespondAsync<IOperationResult<bool>>(result);
    }
  }
}
