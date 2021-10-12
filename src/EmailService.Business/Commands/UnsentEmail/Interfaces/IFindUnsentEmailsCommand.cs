using System.Threading.Tasks;
using LT.DigitalOffice.EmailService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Requests;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EmailService.Business.Commands.UnsentEmail.Interfaces
{
  [AutoInject]
  public interface IFindUnsentEmailsCommand
  {
    Task<FindResultResponse<UnsentEmailInfo>> ExecuteAsync(BaseFindFilter filter);
  }
}
