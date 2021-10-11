using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using System.Threading.Tasks;

namespace LT.DigitalOffice.EmailService.Business.Commands.EmailTemplateText.Interfaces
{
  [AutoInject]
  public interface ICreateEmailTemplateTextCommand
  {
    Task<OperationResultResponse<Guid?>> Execute(EmailTemplateTextRequest request);
  }
}
