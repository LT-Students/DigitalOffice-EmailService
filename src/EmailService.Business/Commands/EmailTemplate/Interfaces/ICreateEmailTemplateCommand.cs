using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EmailService.Business.Commands.EmailTemplate.Interfaces
{
  [AutoInject]
  public interface ICreateEmailTemplateCommand
  {
    Task<OperationResultResponse<Guid?>> ExecuteAsync(EmailTemplateRequest request);
  }
}
