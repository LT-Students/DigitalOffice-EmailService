using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EmailService.Business.Commands.UnsentEmail.Interfaces
{
  [AutoInject]
  public interface IResendEmailCommand
  {
    Task<OperationResultResponse<bool>> Execute(Guid id);
  }
}
