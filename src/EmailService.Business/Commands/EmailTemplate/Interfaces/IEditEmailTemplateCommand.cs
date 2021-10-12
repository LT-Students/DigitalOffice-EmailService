using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplate;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EmailService.Business.Commands.EmailTemplate.Interfaces
{
  [AutoInject]
  public interface IEditEmailTemplateCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(
      Guid emailTemplateId,
      JsonPatchDocument<EditEmailTemplateRequest> patch);
  }
}
