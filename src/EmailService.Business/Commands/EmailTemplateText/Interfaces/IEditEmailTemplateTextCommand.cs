using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.EmailService.Models.Dto.Requests.EmailTemplateText;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;

namespace LT.DigitalOffice.EmailService.Business.Commands.EmailTemplateText.Interfaces
{
  [AutoInject]
  public interface IEditEmailTemplateTextCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(
      Guid emailTemplateTextId,
      JsonPatchDocument<EditEmailTemplateTextRequest> patch);
  }
}
