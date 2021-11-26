using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Models.Broker.Requests.Company;

namespace LT.DigitalOffice.EmailService.Models.Dto.Configurations
{
  public class RabbitMqConfig : BaseRabbitMqConfig
  {
    public string SendEmailEndpoint { get; set; }
    public string CreateSmtpCredentialsEndpoint { get; set; }
    public Dictionary<string, string> FindUserParseEntitiesEndpoint { get; set; }

    [AutoInjectRequest(typeof(IGetSmtpCredentialsRequest))]
    public string GetSmtpCredentialsEndpoint { get; set; }
  }
}
